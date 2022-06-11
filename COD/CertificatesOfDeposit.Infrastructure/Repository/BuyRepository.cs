using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IBuyRepository : IGenericRepository<Buy>
    {
        Task<int> ApproveAsync(Buy entity);
        Task<int> MaxIdAsync();
        Task<PagedResults<Buy>> QueryAsync(string accountClass, string accountNum, string cifNum, string status, string branchCode, DateTime? transactionDate, int pageNum = 0, int pageSize = int.MaxValue);

        Task<List<Buy>> GetAllBySellIdAsync(string sellId);

        Task<Buy> GetTransAdjusmentAsync(string sellId);
    }
    public class BuyRepository : IBuyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public BuyRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> AddAsync(Buy entity)
        {
            var paramInsertBuy = new OracleDynamicParameters();
            paramInsertBuy.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_WAITING_TIME", entity.WaitingTime, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_CREATED_DATE", entity.CreatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_CREATED_BRANCH_CODE", entity.CreatedBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_CREATED_USER", entity.CreatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_SELL_ID", entity.SellId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BOOKING_ID", entity.BookingId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_XREF_ID", entity.XrefId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_SERI_NO", entity.SeriNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_DELETED_USER", entity.DeletedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_DELETED_DATE", entity.DeletedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_ERP_EXTRASTRING", entity.ErpExtrastring, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_ERP_SERIOWNER", entity.ErpSeriowner, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_KPI_INDIRECT", entity.KpiIndirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_KPI_DIRECT", entity.KpiDirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_XREF_FEE_DTTG", entity.XrefFeeDttg, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_DATA", entity.Data, dbType: OracleMappingType.Clob, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_METHOD", entity.BuyPaymentMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_ID_NUM", entity.BuyIdNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_ACCOUNT_NO", entity.BuyPaymentAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_CIF", entity.BuyCif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_FULLNAME", entity.BuyFullname, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PHONE", entity.BuyPhone, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_ADDRESS", entity.BuyAddress, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_ACCOUNT_BALANCE", entity.BuyAccountBalance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_BRANCH_CODE", entity.BuyBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_TELLER_USER", entity.TellerUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_TELLER_DATE", entity.TellerDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_CCY", entity.BuyPaymentCCY, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYOUT_METHOD", entity.BuyPayoutMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYOUT_ACC_NUM", entity.BuyPayoutAccNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BRANCH_ACCOUNT_NO", entity.BranchAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_BRANCH_CODE", entity.BuyPaymentBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input); 
            paramInsertBuy.Add(name: "V_BUY_PAYOUT_BRANCH_CODE", entity.BuyPayoutBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_BALLANCE", entity.BuyPaymentBallance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramInsertBuy.Add(name: "V_BUY_PAYMENT_FEE", entity.BuyPaymentFee, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_BUY_PKG.INSERT_BUY", param: paramInsertBuy, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateAsync(Buy entity)
        {
            var paramUpdateBuy = new OracleDynamicParameters();
            paramUpdateBuy.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_WAITING_TIME", entity.WaitingTime, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_CREATED_DATE", entity.CreatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_CREATED_BRANCH_CODE", entity.CreatedBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_CREATED_USER", entity.CreatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_SELL_ID", entity.SellId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BOOKING_ID", entity.BookingId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_XREF_ID", entity.XrefId, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_SERI_NO", entity.SeriNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_DELETED_USER", entity.DeletedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_DELETED_DATE", entity.DeletedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_ERP_EXTRASTRING", entity.ErpExtrastring, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_ERP_SERIOWNER", entity.ErpSeriowner, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_KPI_INDIRECT", entity.KpiIndirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_KPI_DIRECT", entity.KpiDirect, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_XREF_FEE_DTTG", entity.XrefFeeDttg, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_DATA", entity.Data, dbType: OracleMappingType.Clob, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_METHOD", entity.BuyPaymentMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_ID_NUM", entity.BuyIdNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_ACCOUNT_NO", entity.BuyPaymentAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_CIF", entity.BuyCif, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_FULLNAME", entity.BuyFullname, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PHONE", entity.BuyPhone, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_ADDRESS", entity.BuyAddress, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_ACCOUNT_BALANCE", entity.BuyAccountBalance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_BRANCH_CODE", entity.BuyBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_TELLER_USER", entity.TellerUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_TELLER_DATE", entity.TellerDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_CCY", entity.BuyPaymentCCY, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYOUT_METHOD", entity.BuyPayoutMethod, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYOUT_ACC_NUM", entity.BuyPayoutAccNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BRANCH_ACCOUNT_NO", entity.BranchAccountNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);

            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_BRANCH_CODE", entity.BuyPaymentBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYOUT_BRANCH_CODE", entity.BuyPayoutBranchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);

            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_BALLANCE", entity.BuyAccountBalance, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_BUY_PAYMENT_FEE", entity.BuyPaymentFee, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_BUY_PKG.UPDATE_BUYID", param: paramUpdateBuy, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> ApproveAsync(Buy entity)
        {
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var paramUpdateBuy = new OracleDynamicParameters();
            paramUpdateBuy.Add(name: "V_ID", entity.Id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_TRANSACTION_DATE", entity.TransactionDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_STATUS", entity.Status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_SERI_NO", entity.SeriNo, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_APPROVED_USER", entity.ApprovedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_APPROVED_DATE", entity.ApprovedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_UPDATED_USER", entity.UpdatedUser, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramUpdateBuy.Add(name: "V_UPDATED_DATE", entity.UpdatedDate, dbType: OracleMappingType.Date, direction: ParameterDirection.Input);
            var result = await this._context.Connection.ExecuteAsync("TKGTCG_BUY_PKG.APPROVED_BUYID", param: paramUpdateBuy, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<PagedResults<Buy>> QueryAsync(string accountClass, string accountNum, string cifNum, string status, string branchCode, DateTime? transactionDate, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var results = new PagedResults<Buy>();

            var paramQueryBuy = new OracleDynamicParameters();
            paramQueryBuy.Add(name: "V_CREATED_USER", null, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_BALANCE_FROM", decimal.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_BALANCE_TO", decimal.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_REMAIN_DATE_FROM", int.MinValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_REMAIN_DATE_TO", int.MaxValue, dbType: OracleMappingType.Decimal, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_ACCOUNT_CLASS", accountClass, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_ACCOUNT_NO", accountNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_CIF", cifNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_STATUS", status, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_BRANCH_CODE", branchCode, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_PAGE_NUM", pageNum, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "V_PAGE_SIZE", pageSize, dbType: OracleMappingType.Int32, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "P_BUY_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "P_SELL_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_RECORD", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_CHO_HACH_TOAN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_CHO_DUYET_CN", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_CHUYEN_NHUONG_THANH_CONG", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_TU_CHOI", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "TOTAL_DA_XOA", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_BUY_PKG.GET_PAGING_BUY", param: paramQueryBuy, commandType: CommandType.StoredProcedure))
            {
                results.Items = queryMultiDatas.Read<Buy>().ToList();
                var sells = queryMultiDatas.Read<Sell>().ToList();
                foreach (var item in results.Items)
                {
                    item.Sell = sells.FirstOrDefault(s => s.Id == item.SellId);
                }
                results.TotalCount = paramQueryBuy.Get<int>("TOTAL_RECORD");
                results.TotalStatusBuyChoHanhToan = paramQueryBuy.Get<int>("TOTAL_CHO_HACH_TOAN");
                results.TotalStatusBuyChoDuyetLenhNCN = paramQueryBuy.Get<int>("TOTAL_CHO_DUYET_CN");
                results.TotalStatusBuyChuyenNhuongThanhCong = paramQueryBuy.Get<int>("TOTAL_CHUYEN_NHUONG_THANH_CONG");
                results.TotalStatusBuyTuChoi = paramQueryBuy.Get<int>("TOTAL_TU_CHOI");
                results.TotalStatusBuyXoa = paramQueryBuy.Get<int>("TOTAL_DA_XOA");
            }
            return results;
        }

        public Task<IReadOnlyList<Buy>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Buy> GetByIdAsync(string id)
        {
            var paramQueryBuy = new OracleDynamicParameters();
            paramQueryBuy.Add(name: "V_ID", id, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "P_BUY_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "P_SELL_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_BUY_PKG.GET_DETAIL_BUYID", param: paramQueryBuy, commandType: CommandType.StoredProcedure))
            {
                var data = queryMultiDatas.Read<Buy>().FirstOrDefault();
                data.Sell = queryMultiDatas.Read<Sell>().FirstOrDefault();

                return data;
            }
        }

        public async Task<int> MaxIdAsync()
        {
            var sql = @"select count(t.id) + 1 gencounter from TKGTCG_BUY t";
            var result = await _context.Connection.ExecuteScalarAsync<int>(sql);
            return result;
        }

        public async Task<List<Buy>> GetAllBySellIdAsync(string sellId)
        {
            var sql = @"select A.* from TKGTCG_BUY A join TKGTCG_SELL B ON A.SELL_ID = B.ID WHERE B.ID = :V_SELL_ID ORDER BY A.CREATED_DATE DESC";
            var result = await _context.Connection.QueryAsync<Buy>(sql, new
            {
                v_sell_id = sellId
            });
            return result.ToList();
        }

        public async Task<Buy> GetTransAdjusmentAsync(string accountNum)
        {
            var paramQueryBuy = new OracleDynamicParameters();
            paramQueryBuy.Add(name: "V_ACCOUNT_NO", accountNum, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input);
            paramQueryBuy.Add(name: "P_BUY_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            paramQueryBuy.Add(name: "P_SELL_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            using (var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_MANA_TRANS_PKG.GET_TRANS_ADJUSTMENT", param: paramQueryBuy, commandType: CommandType.StoredProcedure))
            {
                var data = queryMultiDatas.Read<Buy>().FirstOrDefault();
                data.Sell = queryMultiDatas.Read<Sell>().FirstOrDefault();

                return data;
            }
        }
    }
}
