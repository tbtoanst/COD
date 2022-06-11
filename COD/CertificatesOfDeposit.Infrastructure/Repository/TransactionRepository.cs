using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<PagedResults<Transaction>> GetListTransactionAsync(string branchCode, DateTime? fromDate, DateTime? toDate, string contractNo, string status, int pageNum, int pageSize, string accNum);
        Task<PagedResults<Buy>> CleanQueryBuyAsync(int pageNum = 0, int pageSize = int.MaxValue);
        Task<PagedResults<Sell>> CleanQuerySellAsync(int pageNum = 0, int pageSize = int.MaxValue);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IConnectionContext _context;

        private readonly string _packageName = "TKGTCG_TRANSACTION_PKG";

        public TransactionRepository(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IConnectionContext context)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._context = context;
        }

        public async Task<int> AddAsync(Transaction entity)
        {
            try
            {
                var paramInsert = new OracleDynamicParameters();
                paramInsert.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_ACCOUNT_NUM", entity.AccountNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_ACCOUNT_BALANCE", entity.AccountBalance, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_INTEREST", entity.Interest, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_OPEN_DATE", entity.OpenDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_EXPIRED_DATE", entity.ExpiredDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_EXPIRED_DAY", entity.ExpiredDay, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_BUY_INTEREST", entity.BuyInterest, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_TRANSACTION_AMOUNT", entity.TransactionAmount, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_BUY_FEE", entity.BuyFee, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_AMOUNT_FEE", entity.AmountFee, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_BUY_CIF", entity.BuyCif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_BUY_BRANCH_CODE", entity.BuyBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_CREATED_USER", entity.CreatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_CREATED_DATE", entity.CreatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_BUY_ID", entity.BuyId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_CCY", entity.Ccy, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_CUS_FULL_NAME", entity.CusFullName, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_PRIORITY_BRANCH_CODE", entity.PriorityBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_PRIORITY_SELLER", entity.PrioritySeller, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_MATCHED_DATE", entity.MatchedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_ADVANCE_STATUS", entity.AdvanceStatus, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramInsert.Add(name: "V_CONTRACT_NO", entity.ContractNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                var result = await this._context.Connection.ExecuteAsync($"{_packageName}.INSERT_TRANSACTION", param: paramInsert, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Transaction>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResults<Transaction>> GetListTransactionAsync(string branchCode, DateTime? fromDate, DateTime? toDate, string contractNo, string status, int pageNum, int pageSize, string accNum)
        {
            try
            {
                var paramQuery = new OracleDynamicParameters();
                paramQuery.Add(name: "V_BRANCH_CODE", branchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_FROM_DATE", fromDate ?? DateTime.MinValue, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_TO_DATE", toDate ?? DateTime.MaxValue, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_CONTRACT_NO", contractNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_STATUS", status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
                paramQuery.Add(name: "V_ACCOUNT_NUM", accNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);

                paramQuery.Add(name: "P_TRAN_DATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                paramQuery.Add(name: "P_BUY_DATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                paramQuery.Add(name: "P_SELL_DATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                paramQuery.Add(name: "P_BUY_CONTRACT_DATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                paramQuery.Add(name: "P_SELL_CONTRACT_DATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                paramQuery.Add(name: "P_TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

                var data = await this._context.Connection.QueryMultipleAsync($"{_packageName}.GET_LIST_TRANSACTION", param: paramQuery, commandType: CommandType.StoredProcedure);

                var rsTran = data.Read<Transaction>();
                var rsBuys = data.Read<Buy>();
                var rsSells = data.Read<Sell>();
                var rsBuyContract = data.Read<BuyContract>();
                var rsSellContract = data.Read<SellContract>();
                var totalCount = paramQuery.Get<int>("P_TOTAL_RECORD");
                rsTran = rsTran.Select(s =>
                {
                    s.Buy = rsBuys.FirstOrDefault(f => f.Id == s.BuyId);
                    if(s.Buy != null && rsBuyContract != null)
                    {
                        s.Buy.BuyContract = rsBuyContract.FirstOrDefault(f => f.BuyID == s.BuyId);
                    }
                    s.Sell = rsSells.FirstOrDefault(f => f.Id == s.SellId);
                    if (s.Sell != null && rsSellContract != null)
                    {
                        s.Sell.SellContract = rsSellContract.FirstOrDefault(f => f.SellID == s.SellId);
                    }
                    return s;
                });
                return new PagedResults<Transaction>
                {
                    Items = rsTran.ToList(),
                    TotalCount = totalCount
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Transaction entity)
        {
            var sql = @"update tkgtcg_transaction
                           set status = :v_status
                         where id = :v_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                v_status = entity.Status,
                v_id = entity.Id
            });
            return result;
        }

        public async Task<PagedResults<Buy>> CleanQueryBuyAsync(int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Buy>();

            var paramQueryBuy = new OracleDynamicParameters();
            paramQueryBuy.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "P_BUY_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "P_SELL_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_TRANSACTION_PKG.GET_CLEAN_BUY", param: paramQueryBuy, commandType: CommandType.StoredProcedure))
            {
                results.Items = queryMultiDatas.Read<Buy>().ToList();
                var sells = queryMultiDatas.Read<Sell>().ToList();
                foreach (var item in results.Items)
                {
                    item.Sell = sells.FirstOrDefault(s => s.Id == item.SellId);
                }
                results.TotalCount = paramQueryBuy.Get<int>("TOTAL_RECORD");
            }
            return results;
        }

        public async Task<PagedResults<Sell>> CleanQuerySellAsync(int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Sell>();

            var paramQuerySell = new OracleDynamicParameters();
            paramQuerySell.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "P_SELL_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_TRANSACTION_PKG.GET_CLEAN_SELL", param: paramQuerySell, commandType: CommandType.StoredProcedure))
            {
                results.Items = queryMultiDatas.Read<Sell>().ToList();
                results.TotalCount = paramQuerySell.Get<int>("TOTAL_RECORD");
            }
            return results;
        }
    }
}
