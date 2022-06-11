using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface ISellRepository : IGenericRepository<Sell>
    {
        Task<int> ApproveAsync(Sell entity);
        Task<PagedResults<Sell>> QueryToBuyAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string accountClass, string accountNo, string cif, DateTime? transactionDate, string createBranchCode, decimal? interestRate, int pageNum = 0, int pageSize = int.MaxValue);
        Task<PagedResults<Sell>> QueryAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status, string accountClass, string accountNo, string cif, DateTime? transactionDate, string createBranchCode, int pageNum = 0, int pageSize = int.MaxValue);
    }
    public class SellRepository : ISellRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public SellRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> AddAsync(Sell entity)
        {
            var paramInsertSell = new OracleDynamicParameters();
            paramInsertSell.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_BOOKING_ID", entity.BookingId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_CIF", entity.SellCif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_ACCOUNT_NUM", entity.AccountNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_ACCOUNT_BALANCE", entity.AccountBalance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_ACCOUNT_CLASS", entity.AccountClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_REMAIN_DAY", entity.RemainDay, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_PRIORITY_BRANCH_CODE", entity.PriorityBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_PRIORITY_SELLER", entity.PrioritySeller, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_CREATED_DATE", entity.CreatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_CREATED_USER", entity.CreatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_CREATED_BRANCH_CODE", entity.CreatedBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_TELLER_USER", entity.TellerUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_TELLER_DATE", entity.TellerDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_DELETED_USER", entity.DeletedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_DELETED_DATE", entity.DeletedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_KPI_INDIRECT", entity.KpiIndirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_KPI_DIRECT", entity.KpiDirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_FULLNAME", entity.SellFullname, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PHONE", entity.SellPhone, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_ADDRESS", entity.SellAddress, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_METHOD", entity.SellPaymentMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_ID_NUM", entity.SellIdNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_ACCOUNT_NO", entity.SellPaymentAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_BRANCH_CODE", entity.SellBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_DATA", entity.Data, dbType: OracleMappingType.Clob, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_CCY", entity.SellPaymentCCY, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_BRANCH_ACCOUNT_NO", entity.BranchAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_BRANCH_CODE", entity.SellPaymentBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_BALLANCE", entity.SellPaymentBallance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramInsertSell.Add(name: "V_SELL_PAYMENT_FEE", entity.SellPaymentFee, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_SELL_PKG.INSERT_SELL", param: paramInsertSell, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Sell>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResults<Sell>> QueryAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status, string accountClass, string accountNo, string cif, DateTime? transactionDate, string createBranchCode, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Sell>();

            var paramQuerySell = new OracleDynamicParameters();
            paramQuerySell.Add(name: "V_CREATED_USER", null, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BALANCE_FROM", balanceFrom ?? Decimal.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BALANCE_TO", balanceTo ?? Decimal.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_REMAIN_DATE_FROM", remainDateFrom ?? int.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_REMAIN_DATE_TO", remainDateTo ?? int.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_STATUS", status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_ACCOUNT_CLASS", accountClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_ACCOUNT_NO", accountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_TRAN_DATE", transactionDate?.ToString("yyyy-MM-dd"), dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BRANCH_CODE", createBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "PDATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_HACH_TOAN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_DUYET_CN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_NHAN_CN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_NCN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_TU_CHOI", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_DA_XOA", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_SELL_PKG.GET_PAGING_SELL", param: paramQuerySell, commandType: CommandType.StoredProcedure))
            {
                results.Items = queryMultiDatas.Read<Sell>().ToList();
                results.TotalCount = paramQuerySell.Get<int>("TOTAL_RECORD");
                results.TotalStatusSellChoHanhToan = paramQuerySell.Get<int>("TOTAL_CHO_HACH_TOAN");
                results.TotalStatusSellChoDuyetLenhCN = paramQuerySell.Get<int>("TOTAL_CHO_DUYET_CN");
                results.TotalStatusSellChoNhanChuyenNhuong = paramQuerySell.Get<int>("TOTAL_CHO_NHAN_CN");
                results.TotalStatusSellChoDuyenLenhNCN = paramQuerySell.Get<int>("TOTAL_CHO_NCN");
                results.TotalStatusSellTuChoi = paramQuerySell.Get<int>("TOTAL_TU_CHOI");
                results.TotalStatusSellXoa = paramQuerySell.Get<int>("TOTAL_DA_XOA");
            }
            return results;
        }


        public async Task<PagedResults<Sell>> QueryToBuyAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string accountClass, string accountNo, string cif, DateTime? transactionDate, string createBranchCode, decimal? interestRate, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Sell>();

            var paramQuerySell = new OracleDynamicParameters();
            paramQuerySell.Add(name: "V_CREATED_USER", null, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BALANCE_FROM", balanceFrom ?? Decimal.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BALANCE_TO", balanceTo ?? Decimal.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_REMAIN_DATE_FROM", remainDateFrom ?? int.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_REMAIN_DATE_TO", remainDateTo ?? int.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_ACCOUNT_CLASS", accountClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_ACCOUNT_NO", accountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_CIF", cif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_TRAN_DATE", transactionDate?.ToString("yyyy-MM-dd"), dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_BRANCH_CODE", createBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_INTEREST_RATE", interestRate, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "PDATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_HACH_TOAN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_DUYET_CN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_NHAN_CN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_CHO_NCN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_TU_CHOI", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQuerySell.Add(name: "TOTAL_DA_XOA", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_BUY_PKG.GET_PAGING_FOR_SELL", param: paramQuerySell, commandType: CommandType.StoredProcedure))
            {
                results.Items = queryMultiDatas.Read<Sell>().ToList();
                results.TotalCount = paramQuerySell.Get<int>("TOTAL_RECORD");
                results.TotalStatusSellChoHanhToan = paramQuerySell.Get<int>("TOTAL_CHO_HACH_TOAN");
                results.TotalStatusSellChoDuyetLenhCN = paramQuerySell.Get<int>("TOTAL_CHO_DUYET_CN");
                results.TotalStatusSellChoNhanChuyenNhuong = paramQuerySell.Get<int>("TOTAL_CHO_NHAN_CN");
                results.TotalStatusSellChoDuyenLenhNCN = paramQuerySell.Get<int>("TOTAL_CHO_NCN");
                results.TotalStatusSellTuChoi = paramQuerySell.Get<int>("TOTAL_TU_CHOI");
                results.TotalStatusSellXoa = paramQuerySell.Get<int>("TOTAL_DA_XOA");
            }
            return results;
        }

        public async Task<Sell> GetByIdAsync(string id)
        {
            var paramQuerySell = new OracleDynamicParameters();
            paramQuerySell.Add(name: "V_ID", id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQuerySell.Add(name: "PDATAS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryDatas = await this._context.Connection.QuerySingleOrDefaultAsync<Sell>("TKGTCG_SELL_PKG.GET_DETAIL_SELL", param: paramQuerySell, commandType: CommandType.StoredProcedure);
            return queryDatas;
        }

        public async Task<int> UpdateAsync(Sell entity)
        {
            var paramUpdateSell = new OracleDynamicParameters();
            paramUpdateSell.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_BOOKING_ID", entity.BookingId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_CIF", entity.SellCif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_ACCOUNT_NUM", entity.AccountNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_ACCOUNT_BALANCE", entity.AccountBalance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_ACCOUNT_CLASS", entity.AccountClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_REMAIN_DAY", entity.RemainDay, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_PRIORITY_BRANCH_CODE", entity.PriorityBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_PRIORITY_SELLER", entity.PrioritySeller, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_CREATED_DATE", entity.CreatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_CREATED_USER", entity.CreatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_CREATED_BRANCH_CODE", entity.CreatedBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_TELLER_USER", entity.TellerUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_TELLER_DATE", entity.TellerDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_DELETED_USER", entity.DeletedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_DELETED_DATE", entity.DeletedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_KPI_INDIRECT", entity.KpiIndirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_KPI_DIRECT", entity.KpiDirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_FULLNAME", entity.SellFullname, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PHONE", entity.SellPhone, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_ADDRESS", entity.SellAddress, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_METHOD", entity.SellPaymentMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_ID_NUM", entity.SellIdNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_ACCOUNT_NO", entity.SellPaymentAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_BRANCH_CODE", entity.SellBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_DATA", entity.Data, dbType: OracleMappingType.Clob, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_CCY", entity.SellPaymentCCY, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_BRANCH_ACCOUNT_NO", entity.BranchAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_BRANCH_CODE", entity.SellPaymentBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_BALLANCE", entity.SellPaymentBallance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_SELL_PAYMENT_FEE", entity.SellPaymentFee, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_SELL_PKG.UPDATE_SELL", param: paramUpdateSell, commandType: CommandType.StoredProcedure);
            return result;
        }


        public async Task<int> ApproveAsync(Sell entity)
        {
            var paramUpdateSell = new OracleDynamicParameters();
            paramUpdateSell.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateSell.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_SELL_PKG.APPROVED_SELL", param: paramUpdateSell, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
