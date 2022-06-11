using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> AddAsync(UserRole entity);

        /// <summary>
        /// Xóa thông tin theo role id
        /// </summary>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteByRoleAsync(string roleId);

        /// <summary>
        /// Xóa thông tin theo user id
        /// </summary>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteByUserAsync(string userId);

        /// <summary>
        /// Xóa thông tin theo user id và role id
        /// </summary>
        /// <param name="userId">id data info</param>
        /// <param name="roleId">id data info</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string userId, string roleId);
    }
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public UserRoleRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(UserRole entity)
        {
            var sql = @"INSERT INTO auth_user_has_roles 
                         (user_id, role_id)
                        VALUES 
                         (@user_id, @role_id)";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> DeleteAsync(string userId, string roleId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        role_id = @role_id and user_id = @user_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { role_id = roleId, user_id = userId });
            return result;
        }

        public async Task<int> DeleteByRoleAsync(string roleId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        role_id = @role_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { role_id = roleId });
            return result;
        }

        public async Task<int> DeleteByUserAsync(string userId)
        {
            var sql = @"DELETE FROM auth_user_has_roles
                        WHERE 
	                        user_id = @user_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { user_id = userId });
            return result;
        }

    }
}
