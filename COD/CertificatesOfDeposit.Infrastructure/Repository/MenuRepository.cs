using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        /// <summary>
        /// Lấy danh sách role theo menu id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Menu>> GetAllByRoleIdAsync(string roleId);
    }
    public class MenuRepository : IMenuRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public MenuRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(Menu entity)
        {
            //var sql = @"INSERT INTO org_branch 
            //             (id, parent_id, code, name, description, created_at)
            //            VALUES 
            //             (@id, @parent_id, @code, @name, @description, SYSDATE())";

            //var result = await this._context.Connection.ExecuteAsync(sql, entity);
            //return result;
            throw new NotImplementedException();
        }

        public async Task<bool> CheckExistsByCode(string deptCode)
        {
            //var sql = @"SELECT COUNT(*)
            //            FROM org_branch a
            //            WHERE a.code = @code ";
            //var deptCodeExists = await this._context.Connection.ExecuteScalarAsync<bool>(sql,
            //    new { code = deptCode });
            //return deptCodeExists;
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            //var sql = @"DELETE FROM org_branch
            //            WHERE 
            //             id = @id";

            //var result = await this._context.Connection.ExecuteAsync(sql, new { Id = id });
            //return result;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Menu>> GetAllAsync()
        {
        //    var sql = @"SELECT o.* FROM org_branch o";
        //    var result = await this._context.Connection.QueryAsync<Branch>(sql);
        //    return result.ToList();
            throw new NotImplementedException();
    }

        public async Task<IReadOnlyList<Menu>> GetAllByRoleIdAsync(string roleId)
        {
            var sql = @"
                    select distinct m.*
                    from AUTH_menu m
                    START WITH m.menu_id in (

                                            select m.menu_id
                                                from AUTH_menu m
                                                join AUTH_MENU_ROLE_PERMIT rm
                                                on rm.menu_id = m.menu_id
                                                where m.is_actived = 1
                                                and rm.role_id = :v_role_id

                                            )
                    CONNECT BY PRIOR m.menu_parent_id = m.menu_id
                    order by m.order_no
            ";
            var result = await this._context.Connection.QueryAsync<Menu>(sql, new
            {
                v_role_id = roleId
            });
            return result.ToList();
        }

        public async Task<Menu> GetByIdAsync(string id)
        {
            //var sql = @"SELECT o.*
            //            FROM org_branch o
            //            WHERE o.id = @id";
            //var result = await this._context.Connection.QuerySingleOrDefaultAsync<Branch>(sql, new { Id = id });
            //return result;
            throw new NotImplementedException();
        }

        public async Task<PagedResults<Menu>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1)
        {
            //var results = new PagedResults<Branch>();

            //var sql = @"SELECT p.*
            //            FROM org_branch p
            //            WHERE p.is_active = @is_active
            //            LIMIT @page_size OFFSET @page_num; 

            //            SELECT 
            //            COUNT(*)
            //            FROM org_branch p
            //            WHERE p.is_active = @is_active";

            //var queryMultiDatas = await this._context.Connection.QueryMultipleAsync(sql,
            //    new
            //    {
            //        is_active = isActive,
            //        page_size = pageSize,
            //        page_num = pageNum * pageSize
            //    });
            //results.Items = queryMultiDatas.Read<Branch>().ToList();
            //results.TotalCount = queryMultiDatas.ReadFirst<int>();
            //return results;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Menu>> QueryByParentAsync(string parentId)
        {
            //    var sql = @"SELECT o.*
            //                FROM org_branch o
            //                WHERE o.parent_id = @parent_id";
            //    var queryDatas = await this._context.Connection.QueryAsync<Branch>(sql, new { parent_id = parentId });
            //    return queryDatas.ToList();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Menu entity)
        {
            //var sql = @"UPDATE org_branch
            //            SET
            //              code = @code,
            //              name = @name,
            //                 parent_id = @parent_id,
            //              description = @description,
            //                 is_active = @is_active,
            //              updated_at = SYSDATE()
            //            WHERE 
            //              id = @id";
            //var result = await this._context.Connection.ExecuteAsync(sql, entity);
            //return result;
            throw new NotImplementedException();
        }
    }
}
