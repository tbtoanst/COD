using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Sell;
using CertificatesOfDeposit.Models.Sell.Request;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.Account
{
    public interface ITransactionLogService
    {
        Task InsertAsync(TransactionLogModel tran);

    }
    public class TransactionLogService : ITransactionLogService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly ITransactionLogRepository _transactionLogRepo;

        public TransactionLogService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, ITransactionLogRepository transactionLogRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._transactionLogRepo = transactionLogRepo;
        }

        public async Task InsertAsync(TransactionLogModel tran)
        {
            var rs = tran.MapProp<TransactionLogModel, TransactionLog>();
            await this._transactionLogRepo.AddAsync(rs);
        }

    }
}
