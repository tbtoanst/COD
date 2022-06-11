using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IBankPassbookRepository : IGenericRepository<BankPassbookErp>
    {
        Task<BankPassbookErp> GetSerialNumber(string makerAcc, string prefix, string branchCode, string code);
        Task<string> PushSerialNumber(string makerAcc, string prefix, string branchCode, string code, string serialNo);
    }
    public class BankPassbookRepository : IBankPassbookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public BankPassbookRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public Task<int> AddAsync(BankPassbookErp entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<BankPassbookErp>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BankPassbookErp> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<BankPassbookErp> GetSerialNumber(string makerAcc, string prefix, string branchCode, string code)
        {
            try
            {
                var paramQuery = new OracleDynamicParameters();
                paramQuery.Add(name: "PM_GDV", makerAcc, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_SOKYHIEU", prefix, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_MACHINHANH", branchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_MAANCHI", code, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                var queryData = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_ERP_PKG.View_Serino", param: paramQuery, commandType: CommandType.StoredProcedure);
                return queryData.Read<BankPassbookErp>().FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PushSerialNumber(string makerAcc, string prefix, string branchCode, string code, string serialNo)
        {
            try
            {
                var paramQuery = new OracleDynamicParameters();
                paramQuery.Add(name: "PM_GDV", makerAcc, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_SOKYHIEU", prefix, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_MACHINHANH", branchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_MAANCHI", code, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "PM_SERIAL", serialNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);

                paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                var queryData = await this._context.Connection.QueryAsync<string>("TKGTCG_PUSH_ERP_PKG.Push_Serino", param: paramQuery, commandType: CommandType.StoredProcedure);
                return queryData?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int> UpdateAsync(BankPassbookErp entity)
        {
            throw new NotImplementedException();
        }


    }
}
