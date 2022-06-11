using AutoMapper;
using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Models.Sell;
using CertificatesOfDeposit.Models.Sell.Request;
using CertificatesOfDeposit.Models.Transaction;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.Account
{
    public interface IBuyService
    {
        Task InsertAsync(BuyModel buy);
        Task InsertMultiAsync(List<BuyModel> buys);
        Task UpdateAsync(BuyModel buy);
        Task ApproveAsync(BuyModel buy);
        Task<PagedResultModel<BuyModel>> QueryAsync(string accountClass, string accountNum, string cifNum, string status, string branchCode, DateTime? transactionDate, int pageNum = 0, int pageSize = int.MaxValue);
        Task<PagedResultModel<SellModel>> QueryWithSellAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status, string accountClass, string accountNo, string cif, DateTime? transactionDate, string branchCode, decimal? interestRate, int pageNum = 0, int pageSize = int.MaxValue);
        Task<BuyModel> GetByIdAsync(string id);
        Task<BuyModel> GetLastBySellIdAsync(string id);

        Task<BuyModel> GetTransAdjusmentAsync(string accountNum);
    }
    public class BuyService : IBuyService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IBuyRepository _buyRepo;
        private readonly ISellRepository _sellRepo;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IJointHolderRepository _jointHolderRepo;
        private readonly IAccountTranFccRepository _accountTranFccRepo;
        private readonly ITransactionLogRepository _transactionLogRepo;
        private readonly ITransactionLockRepository _transactionLockRepo;

        public BuyService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IBuyRepository buyRepo, ITransactionRepository transactionRepository, ISellRepository sellRepo, IJointHolderRepository jointHolderRepo, IAccountTranFccRepository accountTranFccRepo, ITransactionLogRepository transactionLogRepo, ITransactionLockRepository transactionLockRepository)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._buyRepo = buyRepo;
            this._sellRepo = sellRepo;
            _transactionRepository = transactionRepository;
            _jointHolderRepo = jointHolderRepo;
            _accountTranFccRepo = accountTranFccRepo;
            _transactionLogRepo = transactionLogRepo;
            _transactionLockRepo = transactionLockRepository;
        }

        public async Task InsertAsync(BuyModel buy)
        {
            var isLock = await _transactionLockRepo.GetStatusLock(AppConsts._productCode);
            if (isLock == LockDayConsts.MO)
            {
                var rs = buy.MapProp<BuyModel, Buy>();
                await this._buyRepo.AddAsync(rs);
            }
            else
            {
                throw new Exception("ĐANG KHÓA NGÀY, KHÔNG THỂ THÊM GIAO DỊCH");
            }
        }
        public async Task InsertMultiAsync(List<BuyModel> buys)
        {
            var isLock = await _transactionLockRepo.GetStatusLock(AppConsts._productCode);
            if (isLock == LockDayConsts.MO)
            {
                using (var uow = _unitOfWork.Create())
                {
                    try
                    {
                        foreach (var buy in buys)
                        {
                            var rs = buy.MapProp<BuyModel, Buy>();
                            await this._buyRepo.AddAsync(rs);
                            //Cập nhật lại trạng thái của sell
                            var sellData = await _sellRepo.GetByIdAsync(buy.SellId);
                            sellData.UpdatedUser = buy.CreatedUser;
                            sellData.UpdatedDate = DateTime.Now;
                            TransactionLogModel tranLog = new TransactionLogModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                RootId = buy.Id,
                                Status = buy.Status,
                                CreatedAt = DateTime.Now,
                                CreatedUser = buy.CreatedUser,
                                Type = "BUY",
                                Data = buy.SerializeObject()

                            };
                            var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                            await _sellRepo.UpdateAsync(sellData);
                            await _transactionLogRepo.AddAsync(rsTran);
                        }

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


        public async Task UpdateAsync(BuyModel buy)
        {
            buy.Data = (buy.Data as object)?.SerializeObject();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BuyModel, Buy>();
                cfg.CreateMap<SellModel, Sell>();
            });

            var mapper = configuration.CreateMapper();
            var rs = mapper.Map<BuyModel, Buy>(buy);
           
            TransactionLogModel tranLog = new TransactionLogModel
            {
                Id = Guid.NewGuid().ToString(),
                RootId = buy.Id,
                Status = buy.Status,
                CreatedAt = DateTime.Now,
                CreatedUser = buy.CreatedUser,
                Type = "BUY",
                Data = buy.SerializeObject()

            };
            var rsTran = tranLog.MapProp<TransactionLogModel, TransactionLog>();
            await this._buyRepo.UpdateAsync(rs);
            await _transactionLogRepo.AddAsync(rsTran);
        }

        public async Task ApproveAsync(BuyModel buy)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    buy.Data = (buy.Data as object)?.SerializeObject();

                    var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<BuyModel, Buy>();
                        cfg.CreateMap<SellModel, Sell>();
                        cfg.CreateMap<TransactionModel, Transaction>();
                    });

                    var mapper = configuration.CreateMapper();
                    var rs = mapper.Map<BuyModel, Buy>(buy);

                    await this._buyRepo.ApproveAsync(rs);

                    TransactionLogModel tranLog = new TransactionLogModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        RootId = buy.Id,
                        Status = buy.Status,
                        CreatedAt = DateTime.Now,
                        CreatedUser = buy.CreatedUser,
                        Type = "BUY",
                        Data = buy.SerializeObject()

                    };
                    var rsTranLog = tranLog.MapProp<TransactionLogModel, TransactionLog>();
                    await _transactionLogRepo.AddAsync(rsTranLog);

                }
                catch (Exception ex)
                {
                    await uow.RollBackAsync();
                    throw;
                }
                await uow.CommitAsync();
            }
        }

        public async Task<PagedResultModel<BuyModel>> QueryAsync(string accountClass, string accountNum, string cifNum, string status, string branchCode, DateTime? transactionDate, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var rs = await this._buyRepo.QueryAsync(accountClass, accountNum, cifNum, status, branchCode, transactionDate, pageNum, pageSize);
            return new PagedResultModel<BuyModel>
            {
                Items = rs.Items.Select(s =>
                {
                    var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Buy, BuyModel>();
                        cfg.CreateMap<Sell, SellModel>();
                    });

                    var mapper = configuration.CreateMapper();
                    var data = mapper.Map<Buy, BuyModel>(s);
                    data.Data = s.Data?.DeserializeObject<object>();
                    data.Sell.Data = s.Sell.Data?.DeserializeObject<object>();
                    return data;
                }).ToList(),

                TotalCount  = rs.TotalCount,
                TotalStatusBuyChoHanhToan = rs.TotalStatusBuyChoHanhToan,
                TotalStatusBuyChoDuyetLenhNCN = rs.TotalStatusBuyChoDuyetLenhNCN,
                TotalStatusBuyChuyenNhuongThanhCong = rs.TotalStatusBuyChuyenNhuongThanhCong,
                TotalStatusBuyTuChoi = rs.TotalStatusBuyTuChoi,
                TotalStatusBuyXoa = rs.TotalStatusBuyXoa,
            };
        }

        public async Task<PagedResultModel<SellModel>> QueryWithSellAsync(decimal? balanceFrom, decimal? balanceTo, decimal? remainDateFrom, decimal? remainDateTo, string status, string accountClass, string accountNo, string cif, DateTime? transactionDate, string branchCode, decimal? interestRate, int pageNum = 0, int pageSize = int.MaxValue)
        {
            var rs = await this._sellRepo.QueryToBuyAsync(balanceFrom, balanceTo, remainDateFrom, remainDateTo, accountClass, accountNo, cif, transactionDate, branchCode, interestRate, pageNum, pageSize);
            return new PagedResultModel<SellModel>
            {
                Items = rs.Items.Select(s =>
                {
                    var jointHolders = this._jointHolderRepo.GetAllAsync(s.AccountNum).Result;
                    var data = s.MapProp<Sell, SellModel>();
                    var accInfos = _accountTranFccRepo.GetBuyAllAsync(s.AccountNum).Result.Select(sa => sa.MapProp<AccountTranFcc, AccountTranFccModel>()).Single();

                    accInfos.DSDongChuSoHuu = jointHolders.Select(s=>s.MapProp<JointHolder, JointHolderModel>()).ToList();
                    data.Data = accInfos;
                    return data;
                }).ToList(),

                TotalCount = rs.TotalCount,
                TotalStatusSellChoHanhToan = rs.TotalStatusSellChoHanhToan,
                TotalStatusSellChoDuyetLenhCN = rs.TotalStatusSellChoDuyetLenhCN,
                TotalStatusSellChoNhanChuyenNhuong = rs.TotalStatusSellChoNhanChuyenNhuong,
                TotalStatusSellChoDuyetLenhNCN = rs.TotalStatusSellChoDuyenLenhNCN,
                TotalStatusSellTuChoi = rs.TotalStatusSellTuChoi,
                TotalStatusSellXoa = rs.TotalStatusSellXoa,
            };
        }

        public async Task<BuyModel> GetByIdAsync(string id)
        {
            var s = await this._buyRepo.GetByIdAsync(id); 
            
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Buy, BuyModel>();
                cfg.CreateMap<Sell, SellModel>();
            });

            var mapper = configuration.CreateMapper();
            var data = mapper.Map<Buy, BuyModel>(s);
            data.Data = s.Data?.DeserializeObject<object>();
            data.Sell.Data = s.Sell.Data?.DeserializeObject<object>();
            return data;
        }

        public async Task<BuyModel> GetLastBySellIdAsync(string id)
        {
            var sells = await this._buyRepo.GetAllBySellIdAsync(id);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Buy, BuyModel>();
                cfg.CreateMap<Sell, SellModel>();
            });

            var mapper = configuration.CreateMapper();
            var data = sells.Select(s=>
            {
                var rs = mapper.Map<Buy, BuyModel>(s);
                rs.Data = s.Data?.DeserializeObject<object>();
                return rs;
            }).FirstOrDefault();

            return data;
        }

        public async Task<BuyModel> GetTransAdjusmentAsync(string accountNum)
        {
            var s = await this._buyRepo.GetTransAdjusmentAsync(accountNum);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Buy, BuyModel>();
                cfg.CreateMap<Sell, SellModel>();
            });

            var mapper = configuration.CreateMapper();
            var data = mapper.Map<Buy, BuyModel>(s);
            data.Data = s.Data?.DeserializeObject<object>();
            data.Sell.Data = s.Sell.Data?.DeserializeObject<object>();

            return data;
        }
    }
}
