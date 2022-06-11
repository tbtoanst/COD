using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Sell.Request;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.Account
{
    public interface IBuyContractService
    {
        Task InsertAsync(BuyContractModel buy);
        Task UpdateAsync(BuyContractModel buy);
        Task ApproveAsync(BuyContractModel buy);
        Task DeleteAsync(string id);
        Task<BuyContractModel> GetByIdAsync(string id);
    }
    public class BuyContractService : IBuyContractService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IBuyContractRepository _buyContractRepo;

        public BuyContractService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IBuyContractRepository buyContractRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._buyContractRepo = buyContractRepo;
        }

        public async Task InsertAsync(BuyContractModel buy)
        {
            var rs = buy.MapProp<BuyContractModel, BuyContract>();
            await this._buyContractRepo.AddAsync(rs);
        }

        public async Task<BuyContractModel> GetByIdAsync(string id)
        {
            var rs = await this._buyContractRepo.GetByIdAsync(id);
            return rs.MapProp<BuyContract, BuyContractModel>();
         
        }

        public async Task DeleteAsync(string id)
        {
            await this._buyContractRepo.DeleteAsync(id);

        }

        public async Task UpdateAsync(BuyContractModel buy)
        {
            var rs = buy.MapProp<BuyContractModel, BuyContract>();
            await this._buyContractRepo.UpdateAsync(rs);
        }

        public Task ApproveAsync(BuyContractModel buy)
        {
            throw new NotImplementedException();
        }
    }
}
