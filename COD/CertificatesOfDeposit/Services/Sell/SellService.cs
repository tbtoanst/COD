using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Sell;
using CertificatesOfDeposit.Models;
using CertificatesOfDeposit.Models.Account;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificatesOfDeposit.Models.Sell.Request;
using System.Dynamic;
using CertificatesOfDeposit.Models.Globally;

namespace CertificatesOfDeposit.Services.Account
{
    public interface ISellService
    {
        Task InsertAsync(SellModel sell);
        Task InsertMultiAsync(List<SellModel> sells);
        Task UpdateAsync(SellModel sell);
        Task ApproveAsync(SellModel sell);
        Task<PagedResultModel<SellModel>> QueryAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status,string accountClass,string accountNo,string cif, DateTime? transactionDate, string createBranchCode, int pageNum = 0, int pageSize = int.MaxValue);
        Task<SellModel> GetByIdAsync(string id);
    }
    public class SellService : ISellService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly ISellRepository _sellRepo;
        private readonly IJointHolderRepository _jointHolderRepo;
        private readonly ITransactionLogRepository _transactionLogRepo;
        private readonly ITransactionLockRepository _transactionLockRepo;

        public SellService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, ISellRepository sellRepo, IJointHolderRepository jointHolderRepo, ITransactionLogRepository transactionLogRepo, ITransactionLockRepository transactionLockRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._sellRepo = sellRepo;
            this._jointHolderRepo = jointHolderRepo;
            _transactionLogRepo = transactionLogRepo;
            _transactionLockRepo = transactionLockRepo;
        }

        public async Task InsertAsync(SellModel sell)
        {
            var isLock = await _transactionLockRepo.GetStatusLock(AppConsts._productCode);
            if (isLock == LockDayConsts.MO)
            {
                using (var uow = _unitOfWork.Create())
                {
                    try
                    {
                        sell.Id = Guid.NewGuid().ToString();
                        var sells = sell.MapProp<SellModel, Sell>();
                        TransactionLogModel tranLog = new TransactionLogModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            RootId = sell.Id,
                            Status = sell.Status,
                            CreatedAt = DateTime.Now,
                            CreatedUser = sell.CreatedUser,
                            Type = "SELL",
                            Data = sell.SerializeObject()

                        };
                        var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                        await this._sellRepo.AddAsync(sells);

                        await _transactionLogRepo.AddAsync(rsTran);
                        await uow.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await uow.RollBackAsync();
                        throw;
                    }
                }
            }
            else
            {
                throw new Exception("ĐANG KHÓA NGÀY, KHÔNG THỂ THÊM GIAO DỊCH");
            }
        }


        public async Task UpdateAsync(SellModel sell)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    sell.Data = (sell.Data as object)?.SerializeObject();

                    var rs = sell.MapProp<SellModel, Sell>();
                    TransactionLogModel tranLog = new TransactionLogModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        RootId = sell.Id,
                        Status = sell.Status,
                        CreatedAt = DateTime.Now,
                        CreatedUser = sell.CreatedUser,
                        Type = "SELL",
                        Data = sell.SerializeObject()

                    };
                    var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                    await this._sellRepo.UpdateAsync(rs);
                    await _transactionLogRepo.AddAsync(rsTran);
                    await uow.CommitAsync();
                }
                catch (Exception ex)
                {
                    await uow.RollBackAsync();
                    throw;
                }
            }
        }

        public async Task ApproveAsync(SellModel sell)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    sell.Data = (sell.Data as object)?.SerializeObject();

                    var rs = sell.MapProp<SellModel, Sell>();
                    TransactionLogModel tranLog = new TransactionLogModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        RootId = sell.Id,
                        Status = sell.Status,
                        CreatedAt = DateTime.Now,
                        CreatedUser = sell.CreatedUser,
                        Type = "SELL",
                        Data = sell.SerializeObject()

                    };
                    var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                    await this._sellRepo.ApproveAsync(rs);
                    await _transactionLogRepo.AddAsync(rsTran);
                    await uow.CommitAsync();
                }
                catch (Exception ex)
                {
                    await uow.RollBackAsync();
                    throw;
                }
            }
            
        }

        public async Task<PagedResultModel<SellModel>> QueryAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status, string accountClass, string accountNo, string cif, DateTime? transactionDate, string createBranchCode, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var rs = await this._sellRepo.QueryAsync(balanceFrom, balanceTo, remainDateFrom, remainDateTo, status, accountClass, accountNo, cif, transactionDate, createBranchCode, pageNum, pageSize);
            return new PagedResultModel<SellModel>
            {
                Items = rs.Items.Select( s =>
                {
                    var jointHolders = this._jointHolderRepo.GetAllAsync(s.AccountNum).Result;
                    var data = s.MapProp<Sell, SellModel>();
                    data.Data = s.Data.DeserializeObject<ExpandoObject>();
                    data.Data.ds_dong_chu_so_huu = jointHolders.ToList();
                    return data;
                }).ToList(),
                TotalCount = rs.TotalCount,
                TotalStatusSellChoHanhToan = rs.TotalStatusSellChoHanhToan,
                TotalStatusSellChoDuyetLenhCN = rs.TotalStatusSellChoDuyetLenhCN,
                TotalStatusSellChoNhanChuyenNhuong = rs.TotalStatusSellChoNhanChuyenNhuong,
                TotalStatusSellChoDuyetLenhNCN = rs.TotalStatusSellChoDuyenLenhNCN,
                TotalStatusSellTuChoi = rs.TotalStatusSellTuChoi,
                TotalStatusSellXoa = rs.TotalStatusSellXoa
            };
        }

        public async Task<SellModel> GetByIdAsync(string id)
        {
            var s = await this._sellRepo.GetByIdAsync(id);

            var jointHolders = this._jointHolderRepo.GetAllAsync(s.AccountNum).Result;
            var data = s.MapProp<Sell, SellModel>();
            data.Data = s.Data.DeserializeObject<ExpandoObject>();
            data.Data.ds_dong_chu_so_huu = jointHolders.ToList();
            return data;

        }

        public async Task InsertMultiAsync(List<SellModel> sells)
        {
            var isLock = await _transactionLockRepo.GetStatusLock(AppConsts._productCode);
            if (isLock == LockDayConsts.MO)
            {
                using (var uow = _unitOfWork.Create())
                {
                    try
                    {
                        foreach (var sell in sells)
                        {
                            sell.Id = Guid.NewGuid().ToString();
                            var sellData = sell.MapProp<SellModel, Sell>();
                            TransactionLogModel tranLog = new TransactionLogModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                RootId = sell.Id,
                                Status = sell.Status,
                                CreatedAt = DateTime.Now,
                                CreatedUser = sell.CreatedUser,
                                Type = "SELL",
                                Data = sell.SerializeObject()

                            };
                            var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                            await this._sellRepo.AddAsync(sellData);
                            await _transactionLogRepo.AddAsync(rsTran);
                        }
                        await uow.CommitAsync();

                    }
                    catch (Exception)
                    {
                        await uow.RollBackAsync();
                        throw;
                    }
                }
                
            }
            else
            {
                throw new Exception("ĐANG KHÓA NGÀY, KHÔNG THỂ THÊM GIAO DỊCH");
            }
        }
    }
}
