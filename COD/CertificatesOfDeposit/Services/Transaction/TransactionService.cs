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
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CertificatesOfDeposit.Services.Transactions
{
    public interface ITransactionService
    {
        /// <summary>
        /// Insert a new transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<int> AddAsync(TransactionModel transaction);

        Task<PagedResultModel<TransactionModel>> GetListAsync(string branchCode, DateTime? fromDate, DateTime? toDate, string contractNo, string status, int pageNum, int pageSize, string accNum);

        Task<List<StepStatusModel>> GetStartStepAsync(string productCode, string landCode, string phaseCode);
        Task<List<StepStatusModel>> GetNextStepAsync(string id, string productCode, string landCode);
        Task<List<StepStatusModel>> GetDeleteStepAsync(string id, string productCode, string landCode);
        Task<List<StepStatusModel>> GetApproveStepAsync(string id, string productCode, string landCode);
        Task<List<StepStatusModel>> GetRejectStepAsync(string id, string productCode, string landCode);
        Task<string> GetStatusLockAsync();
        Task<int> UpdateTransactionLock(string id, string userUpdate, string status);
        Task Update(string id);
        Task<PagedResultModel<BuyModel>> CleanQueryBuyAsync(int pageNum = 0, int pageSize = int.MaxValue);
        Task<PagedResultModel<SellModel>> CleanQuerySellAsync(int pageNum = 0, int pageSize = int.MaxValue);
        Task<string> ExcecuteCleanEndDayAsync(string userAction);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _config;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IStepStatusRepository _stepStatusRepository;
        private readonly ITransactionLockRepository _transactionLockRepository;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IBuyRepository _buyRepo;
        private readonly ISellRepository _sellRepo;

        public TransactionService(IConfiguration configuration, ITransactionRepository transactionRepository, IStepStatusRepository stepStatusRepository, ITransactionLockRepository transactionLockRepository, IUnitOfWorkContext unitOfWork,IBuyRepository buyRepository, ISellRepository sellRepository)
        {
            this._config = configuration;
            this._transactionRepository = transactionRepository;
            this._stepStatusRepository = stepStatusRepository;
            _transactionLockRepository = transactionLockRepository;
            _unitOfWork = unitOfWork;
            _buyRepo = buyRepository;
            _sellRepo = sellRepository;
        }

        public async Task<int> AddAsync(TransactionModel transaction)
        {
            try
            {
                var trans = transaction.MapProp<TransactionModel, Transaction>();
                var result = await _transactionRepository.AddAsync(trans);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetStatusLockAsync()
        {
            try
            {
                var result = await _transactionLockRepository.GetStatusLock(AppConsts._productCode);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<StepStatusModel>> GetApproveStepAsync(string id, string productCode, string landCode)
        {
            try
            {
                var result = await _stepStatusRepository.GetApproveStepAsync(id, productCode);
                return result.Where(s => s.LaneCode == landCode)
                             .Select(s => s.MapProp<StepStatus, StepStatusModel>())
                             .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(string id)
        {
            try
            {
                TransactionModel transaction = new TransactionModel();
                transaction.Id = id;
                transaction.Status = ContractConsts.DA_DUYET;
                var data = transaction.MapProp<TransactionModel, Transaction>();
                var result = await _transactionRepository.UpdateAsync(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StepStatusModel>> GetDeleteStepAsync(string id, string productCode, string landCode)
        {
            try
            {
                var result = await _stepStatusRepository.GetDeleteStepAsync(id, productCode);
                return result.Where(s => s.LaneCode == landCode)
                             .Select(s => s.MapProp<StepStatus, StepStatusModel>())
                             .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResultModel<TransactionModel>> GetListAsync(string branchCode, DateTime? fromDate, DateTime? toDate, string contractNo, string status, int pageNum, int pageSize, string accNum)
        {
            try
            {
                var ouput = await _transactionRepository.GetListTransactionAsync(branchCode, fromDate, toDate, contractNo, status, pageNum, pageSize, accNum);
                return new PagedResultModel<TransactionModel>
                {
                    Items = ouput.Items.Select(s =>
                    {
                        var configuration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Transaction, TransactionModel>();
                            cfg.CreateMap<Buy, BuyModel>();
                            cfg.CreateMap<Sell, SellModel>();
                            cfg.CreateMap<BuyContract, BuyContractModel>();
                            cfg.CreateMap<SellContract, SellContractModel>();
                        });

                        var mapper = configuration.CreateMapper();
                        var tran = mapper.Map<Transaction, TransactionModel>(s);

                        tran.Sell.Data = s.Sell?.Data?.DeserializeObject<object>();
                        tran.Buy.Data = s.Buy?.Data?.DeserializeObject<object>();

                        return tran;

                    }).ToList(),
                    TotalCount = ouput.TotalCount
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<StepStatusModel>> GetNextStepAsync(string id, string productCode, string landCode)
        {
            try
            {
                var result = await _stepStatusRepository.GetNextStepAsync(id, productCode);
                return result.Where(s => s.LaneCode == landCode)
                             .Select(s => s.MapProp<StepStatus, StepStatusModel>())
                             .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StepStatusModel>> GetRejectStepAsync(string id, string productCode, string landCode)
        {
            try
            {
                var result = await _stepStatusRepository.GetRejectStepAsync(id, productCode);
                return result.Where(s => s.LaneCode == landCode)
                             .Select(s => s.MapProp<StepStatus, StepStatusModel>())
                             .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StepStatusModel>> GetStartStepAsync(string productCode, string landCode, string phaseCode)
        {
            try
            {
                var result = await _stepStatusRepository.GetStartStepAsync(productCode);
                return result.Where(s => s.LaneCode == landCode && s.PhaseCode == phaseCode)
                             .Select(s => s.MapProp<StepStatus, StepStatusModel>()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateTransactionLock(string id, string userUpdate, string status)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    var result = await _transactionLockRepository.UpdateTransactionLock(id, userUpdate, status);
                    TransactionLogModel tranLog = new TransactionLogModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        //RootId = null,
                        Status = status,
                        CreatedAt = DateTime.Now,
                        CreatedUser = userUpdate,
                        Type = "LOCK_TRAN",
                        Data = result
                    };
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<PagedResultModel<BuyModel>> CleanQueryBuyAsync(int pageNum = 0, int pageSize = int.MaxValue)
        {
            var rs = await this._transactionRepository.CleanQueryBuyAsync(pageNum, pageSize);
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

                TotalCount = rs.TotalCount,
            };
        }

        public async Task<PagedResultModel<SellModel>> CleanQuerySellAsync(int pageNum = 0, int pageSize = int.MaxValue)
        {
            var rs = await this._transactionRepository.CleanQuerySellAsync(pageNum, pageSize);
            return new PagedResultModel<SellModel>
            {
                Items = rs.Items.Select(s =>
                {
                    var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Sell, SellModel>();
                    });

                    var mapper = configuration.CreateMapper();
                    var data = mapper.Map<Sell, SellModel>(s);
                    data.Data = s.Data?.DeserializeObject<object>();
                    return data;
                }).ToList(),

                TotalCount = rs.TotalCount,
            };
        }

        public async Task<string> ExcecuteCleanEndDayAsync(string userAction)
        {
            using (var uow = _unitOfWork.Create())
            {
                try
                {
                    var isLockEndDay = await _transactionLockRepository.GetStatusLock(AppConsts._productCode);
                    if (isLockEndDay == LockDayConsts.DONG)
                    {
                        var listBuy = await _transactionRepository.CleanQueryBuyAsync(0, int.MaxValue);
                        var listSell = await _transactionRepository.CleanQuerySellAsync(0, int.MaxValue);

                        TransactionLogModel tranLog = new TransactionLogModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            Status = "XOA_LENH_TREO_CUOI_NGAY",
                            CreatedAt = DateTime.Now,
                            CreatedUser = userAction,
                            Type = "LOCK_TRAN",
                            Data = listBuy.SerializeObject() + listSell.SerializeObject(),
                        };

                        foreach (var itemBuy in listBuy.Items)
                        {
                            itemBuy.UpdatedDate = DateTime.Now;
                            itemBuy.UpdatedUser = userAction;
                            itemBuy.Status = "XOA_LENH_MUA";
                            await this._buyRepo.UpdateAsync(itemBuy);
                        }

                        foreach (var itemSell in listSell.Items)
                        {
                            itemSell.UpdatedDate = DateTime.Now;
                            itemSell.UpdatedUser = userAction;
                            itemSell.Status = "XOA_LENH_BAN";
                            await this._sellRepo.UpdateAsync(itemSell);
                        }
                        return "Xóa cuối ngày hoàn tất!";
                    }
                    throw new Exception("Chưa thực hiện khóa cuối ngày!");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
