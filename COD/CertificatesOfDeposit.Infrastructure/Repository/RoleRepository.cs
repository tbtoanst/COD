using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        /// <summary>
        /// Lấy danh sách Role phân trang
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedResults<Role>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Lấy danh sách Role theo permision
        /// </summary>
        /// <param name="permisionId"></param>
        /// <returns></returns>
        Task<List<Role>> GetRoleByPermissionsAsync(string permisionId);

        /// <summary>
        /// Lấy danh sách Role theo user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Role>> GetRoleByUserAsync(string userId);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public RoleRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(Role entity)
        {
            var sql = @"INSERT INTO auth_Roles 
                         (id, name, guard_name, created_at)
                        VALUES 
                         (@id, @name, @guard_name, SYSDATE())";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = @"DELETE FROM app_Role
                        WHERE 
	                        id = @id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { Id = id });
            return result;
        }

        public async Task<IReadOnlyList<Role>> GetAllAsync()
        {
            var sql = @"SELECT r.* FROM auth_roles r";
            var result = await this._context.Connection.QueryAsync<Role>(sql);
            return result.ToList();
        }

        public async Task<Role> GetByIdAsync(string id)
        {
            var sql = @"SELECT p.* FROM auth_role p
                        WHERE p.role_id = :v_id";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<Role>(sql, new { v_id = id });
            return result;
        }

        public async Task<List<Role>> GetRoleByPermissionsAsync(string permisionId)
        {
            var sql = @"SELECT 
                        p.id,
                        p.name,
                        p.guard_name,
                        p.created_at,
                        p.updated_at
                        FROM 
                        auth_role_has_permissions r JOIN auth_roles p ON r.role_id = p.id
                        WHERE r.permission_id = @permission_id";
            var result = await this._context.Connection.QueryAsync<Role>(sql, new { permission_id = permisionId });
            return result.ToList();
        }
        public async Task<List<Role>> GetRoleByUserAsync(string userId)
        {
            var sql = @"SELECT 
                         r.id,
                         r.name,
                         r.guard_name,
                         r.created_at,
                         r.updated_at
                        FROM 
                         auth_user_has_roles u
                        JOIN auth_roles r ON u.role_id = r.id
                        WHERE u.user_id = @user_id";
            var result = await this._context.Connection.QueryAsync<Role>(sql, new { user_id = userId });
            return result.ToList();
        }

        public async Task<PagedResults<Role>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Role>();

            var sql = @"SELECT p.*
                        FROM auth_roles p
                        LIMIT @page_size OFFSET @page_num; 
                        
                        SELECT 
                        COUNT(*)
                        FROM auth_roles p";

            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync(sql,
                new
                {
                    page_size = pageSize,
                    page_num = pageNum * pageSize
                });
            results.Items = queryMultiDatas.Read<Role>().ToList();
            results.TotalCount = queryMultiDatas.ReadFirst<int>();
            return results;
        }

        public async Task<int> UpdateAsync(Role entity)
        {
            var sql = @"UPDATE auth_roles
                        SET 
	                        name = @name,
	                        guard_name = @guard_name,
                            is_default = @is_default,
	                        updated_at = SYSDATE()
                        WHERE 
	                        id = @id";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}
