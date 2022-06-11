using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models;
using CertificatesOfDeposit.Models.Account;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.Account
{
    public interface IRoleService
    {
        Task<bool> IsValidUserAsync(string user, string password);
        Task<List<string>> GetRoleAsync(string user, string password, string appid);
        Task<UserModel> GetUserInfoAsync(string user, string password);
        Task<PositionModel> GetPostionAsync(string userName);
        Task<List<DepartmentModel>> GetDepartments();
        Task<List<MenuModel>> GetMenusAsync(List<string> roleIds);
        Task<bool> IsRequiredCheckExistsFcc(string roleID);
    }
    public class RoleService : IRoleService
    {
        private readonly IConfiguration _config;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IBranchRepository _branchRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly IPermitRepository _permitRepo;
        private readonly IRoleRepository _roleRepository;
        public RoleService(IConfiguration configuration, IDepartmentRepository departmentRepo, IBranchRepository branchRepo, IMenuRepository menuRepo, IPermitRepository permitRepo, IRoleRepository roleRepository)
        {
            this._config = configuration;
            this._departmentRepo = departmentRepo;
            this._branchRepo = branchRepo;
            this._menuRepo = menuRepo;
            this._permitRepo = permitRepo;
            this._roleRepository = roleRepository;
        }

        public async Task<List<string>> GetRoleAsync(string userName, string password, string appid)
        {
            var url = _config["App:UrlAuth"];

            RestClient restClient = new RestClient($"{url}/api/GetMenuAndPagesByProject");
            RestRequest request = new RestRequest();
            request.AddHeader("USER_NAME", userName);
            request.AddHeader("PASS_WORD", password);
            request.AddHeader("PROJECT_CODE", appid);

            var response = await restClient.ExecuteAsync(request);

            return response.Content.DeserializeObject<Dictionary<string, object>>()["DATA_MENU"].MapProp<List<Dictionary<string, object>>>().Select(s => s["CODE"].ToString()).ToList();
        }

        public async Task<UserModel> GetUserInfoAsync(string userName, string password)
        {
            var url = _config["App:UrlAuth"];

            var pass = _config["App:PassDefault"];

            var authUrl = $"{url}/api/CheckLogInAD";
            if (password.Equals(pass))
            {
                authUrl = $"{url}/api/CheckLogIn";
            }

            RestClient restClient = new RestClient(authUrl);
            RestRequest request = new RestRequest();
            request.AddHeader("USER_NAME", userName);
            request.AddHeader("PASS_WORD", password);

            var response = await restClient.ExecuteAsync(request);

            var dataLogin = response.Content.DeserializeObject<Dictionary<string, object>>();

            if (dataLogin["STATUS"].ToString() == "1")
            {
                var userInfo = dataLogin["USER_INFO"].MapProp<List<Dictionary<string, object>>>().First();

                return new UserModel
                {
                    Id = userInfo["EMP_ID"].ToString(),
                    Name = (string)userInfo["USER_NAME"],
                    FullName = (string)userInfo["FULL_NAME"],
                    Email = (string)userInfo["EMAIL"],
                    //Role = userInfo["GROUP_ID"].ToString()
                };
            }
            return null;
        }

        public async Task<bool> IsValidUserAsync(string userName, string password)
        {
            var url = _config["App:UrlAuth"];

            RestClient restClient = new RestClient($"{url}/api/CheckLogIn");
            RestRequest request = new RestRequest();
            request.AddHeader("USER_NAME", userName);
            request.AddHeader("PASS_WORD", password);

            var response = await restClient.ExecuteAsync(request);

            var dataLogin = response.Content.DeserializeObject<Dictionary<string, object>>();

            if (int.Parse(dataLogin["STATUS"].ToString()) == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<List<DepartmentModel>> GetDepartments()
        {
            var departments = await _departmentRepo.GetAllAsync();

            return departments.Select(s => s.MapProp<Department, DepartmentModel>()).ToList();
        }

        public async Task<PositionModel> GetPostionAsync(string userName)
        {
            var branchs = await _branchRepo.GetAllByUserIdAsync(userName);
            var departments = await _departmentRepo.GetAllByUserIdAsync(userName);
            if (branchs.Any() || departments.Any())
            {
                return new PositionModel
                {
                    Branch = branchs.FirstOrDefault()?.MapProp<Branch, BranchModel>(),
                    Department = departments.FirstOrDefault()?.MapProp<Department, DepartmentModel>(),
                };
            }
            return null;
        }
        public async Task<List<MenuModel>> GetMenusAsync(List<string> roleIds)
        {
            var menus = new List<MenuModel>();

            foreach (var roleId in roleIds)
            {
                var roleMenus = (await _menuRepo.GetAllByRoleIdAsync(roleId)).Select(s => s.MapProp<Menu, MenuModel>()).ToList();
                foreach (var roleMenu in roleMenus)
                {
                    var permits = await _permitRepo.GetAllPermitAsync(roleId, roleMenu.Id);
                    roleMenu.Permits = permits.Select(s => s.MapProp<Permit, PermitModel>()).ToList();
                }
                menus.AddRange(roleMenus);
            }

            //var sql = @"
            //    with tb_role as
            //        (SELECT value
            //        FROM json_table((select :v_role doc from dual),
            //                        '$[*]' COLUMNS(value PATH '$'))),
            //    tb_menu AS
            //        (

            //        select distinct *
            //        from AUTH_menu m
            //        START WITH m.menu_id in (

            //                                select m.menu_id
            //                                    from AUTH_menu m
            //                                    join AUTH_MENU_ROLE_PERMIT rm
            //                                    on rm.menu_id = m.menu_id
            //                                    where m.is_actived = 1
            //                                    and rm.role_id in (select value from tb_role)

            //                                )
            //        CONNECT BY PRIOR m.menu_parent_id = m.menu_id)

            //    select m.* from tb_menu m order by m.order_no
            //";
            //var dataMenus = await _conn.QueryAsync(sql, new { v_role = roles.SerializeObject() });

            //foreach (var dataMenu in dataMenus.ToDict().MapProp<List<IDictionary<string, string>>>().Where(w => w.Count() > 0))
            //{
            //    var menu = new MenuInfoModel()
            //    {
            //        Id = (string)dataMenu["MENU_ID"],
            //        ParentId = (string)dataMenu["MENU_PARENT_ID"],
            //        Name = (string)dataMenu["MENU_NAME"],
            //        Url = (string)dataMenu["URL"],
            //        Icon = (string)dataMenu["ICON"],
            //        Permits = await GetPermitAsync((string)dataMenu["MENU_ID"], roles)
            //    };

            //    menus.Add(menu);
            //}

            return menus.Distinct().ToList();
        }

        public async Task<List<PermissionModel>> GetPermitAsync(string menuId, List<string> roles)
        {
            //var sqlPermit = @"with tb_role as
            //                (SELECT value
            //                FROM json_table((select :v_role doc from dual),
            //                                '$[*]' COLUMNS(value PATH '$')))
            //            SELECT DISTINCT permit_id        AS id,
            //                            permit_parent_id AS parentid,
            //                            permit_action    AS action,
            //                            permit_name      AS name
            //                FROM AUTH_permit p
            //                START WITH p.permit_id IN (SELECT permit_id
            //                                            FROM AUTH_MENU_ROLE_PERMIT
            //                                            WHERE role_id in (select value from tb_role)
            //                                            AND menu_id = :v_menuId)
            //            CONNECT BY PRIOR p.permit_parent_id = p.permit_id";
            //var rsPermit = await _conn.QueryAsync(sqlPermit, new { v_role = roles.SerializeObject(), v_menuId = menuId });
            //var rsDatas = rsPermit.ToDict().MapProp<List<IDictionary<string, string>>>().Where(w => w.Count() > 0).Select(s =>
            //          new PermitModel
            //          {
            //              Id = s["ID"],
            //              ParentId = s["PARENTID"],
            //              Action = s["ACTION"],
            //              Name = s["NAME"]
            //          }).ToList();
            //return rsDatas;
            return new List<PermissionModel>();
        }

        public async Task<bool> IsRequiredCheckExistsFcc(string roleID)
        {
            try
            {
                var strRole = roleID?.Trim().ToUpper();
                var role = await _roleRepository.GetByIdAsync(strRole);
                if (role != null)
                {
                    if (role.RequiredExistsFcc == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
