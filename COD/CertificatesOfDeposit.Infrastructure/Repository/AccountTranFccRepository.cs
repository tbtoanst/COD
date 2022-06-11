using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IAccountTranFccRepository : IGenericRepository<AccountTranFcc>
    {
        /// <summary>
        /// Lấy thông tin giao dịch bán trên core
        /// </summary>
        /// <param name="cif"></param>
        /// <returns></returns>
        Task<IReadOnlyList<AccountTranFcc>> GetAllAsync(string cif);
        
        /// <summary>
        /// Lấy thông tin giao dịch mua trên core
        /// </summary>
        /// <param name="cif"></param>
        /// <returns></returns>
        Task<IReadOnlyList<AccountTranFcc>> GetBuyAllAsync(string cif);
        Task<IReadOnlyList<AccountDetailCASA>> GetAccountDetailsAsync(string cif);
    }
    public class AccountTranFccRepository : IAccountTranFccRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public AccountTranFccRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public Task<int> AddAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(AccountTranFcc id)
        {
            throw new NotImplementedException();
        }
        public Task<int> UpdateAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> ApproveAsync(AccountTranFcc entity)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<AccountTranFcc>> QueryAsync(string userCreate,string status,int pageNum = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<AccountTranFcc>> GetAllAsync(string cif)
        { 
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_ACCOUNT_BY_CIF", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountTranFcc>().ToList();
        }

        public async Task<IReadOnlyList<AccountTranFcc>> GetBuyAllAsync(string cif)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_ACCOUNT_BUY_BY_CIF", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountTranFcc>().ToList();
        }

        public async Task<IReadOnlyList<AccountDetailCASA>> GetAccountDetailsAsync(string cif)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ACCOUNT_DETAIL", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<AccountDetailCASA>().ToList();
        }

        Task<AccountTranFcc> IGenericRepository<AccountTranFcc>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AccountTranFcc>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
