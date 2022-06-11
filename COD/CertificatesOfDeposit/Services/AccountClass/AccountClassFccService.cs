using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Fcc;
using CertificatesOfDeposit.Models.Sell.Request;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.Account
{
    public interface IAccountClassFccService
    {
        Task<AccountClassFccModel> QueryAsync(string accClass);
        Task<List<AccountClassFccModel>> GetAllAsync();
    }
    public class AccountClassFccService : IAccountClassFccService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IAccountClassFccRepository _accounClassRepo;

        public AccountClassFccService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IAccountClassFccRepository accounTranRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._accounClassRepo = accounTranRepo;
        }

        public async Task<List<AccountClassFccModel>> GetAllAsync()
        {
            var rs = await this._accounClassRepo.GetAllAsync();
            return rs.Select(s => s.MapProp<AccountClassFcc, AccountClassFccModel>()).ToList();
            
        }

        public async Task<AccountClassFccModel> QueryAsync(string accClass)
        {
            var rs = await this._accounClassRepo.GetByIdAsync(accClass);
            return rs.MapProp<AccountClassFcc, AccountClassFccModel>();
        }
    }
}
