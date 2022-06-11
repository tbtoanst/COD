using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<IReadOnlyList<Partner>> GetAllAsync(string type);
    }
    public class PartnerRepository : IPartnerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public PartnerRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public Task<int> AddAsync(Partner entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Partner>> GetAllAsync(string type )
        {
            var sql = @"select t.* from TKGTCG_PARTNER t WHERE T.IS_ACTIVE = 1 and (t.type = :v_type or :v_type IS NULL)";
            var result = await this._context.Connection.QueryAsync<Partner>(sql, new
            {
                v_type = type
            });
            return result.ToList();
        }

        public Task<IReadOnlyList<Partner>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Partner> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Partner entity)
        {
            throw new NotImplementedException();
        }
    }
}
