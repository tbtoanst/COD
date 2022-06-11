using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IAccountClassFccRepository : IGenericRepository<AccountClassFcc>
    {
        Task<IReadOnlyList<AccountClassFcc>> GetAllAsync();

    }
    public class AccountClassFccRepository : IAccountClassFccRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public AccountClassFccRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public Task<int> AddAsync(AccountClassFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AccountClassFcc>> GetAllAsync()
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_ACCOUNT_CLASS", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountClassFcc>().ToList();
        }

        public async Task<AccountClassFcc> GetByIdAsync(string accClass)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_ACCOUNT_CLASS_CODE", accClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryData = await this._context.Connection.QuerySingleOrDefaultAsync<AccountClassFcc>("TKGTCG_PUSH_CORE_PKG.GET_ACCOUNT_CLASS_DETAIL", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryData;
        }

        public Task<int> UpdateAsync(AccountClassFcc entity)
        {
            throw new NotImplementedException();
        }
    }
}
