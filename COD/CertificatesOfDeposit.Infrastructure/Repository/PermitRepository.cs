using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IPermitRepository : IGenericRepository<Permit>
    {
        /// <summary>
        /// Lấy danh sách Permit theo role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Permit>> GetAllPermitAsync(string roleId, string menuId);
    }
    public class PermitRepository : IPermitRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public PermitRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public Task<int> AddAsync(Permit entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Permit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Permit> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Permit>> GetAllPermitAsync(string roleId, string menuId)
        {
            var sql = @"
                            SELECT DISTINCT p.*
                                FROM AUTH_permit p
                                START WITH p.permit_id IN (SELECT permit_id
                                                            FROM AUTH_MENU_ROLE_PERMIT
                                                            WHERE role_id = :v_role_id
                                                            AND menu_id = :v_menu_id)
                            CONNECT BY PRIOR p.permit_parent_id = p.permit_id";
            var result = await this._context.Connection.QueryAsync<Permit>(sql, new { v_role_id = roleId, v_menu_id = menuId });
            return result.ToList();
        }

        public Task<int> UpdateAsync(Permit entity)
        {
            throw new NotImplementedException();
        }
    }
}
