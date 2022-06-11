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
    public interface ISellContractService
    {
        Task InsertAsync(SellContractModel sell);
        Task UpdateAsync(SellContractModel sell);
        Task ApproveAsync(SellContractModel sell);
        Task DeleteAsync(string id);
        Task<SellContractModel> GetByIdAsync(string id);
    }
    public class SellContractService : ISellContractService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly ISellContractRepository _sellContractRepo;

        public SellContractService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, ISellContractRepository sellContractRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._sellContractRepo = sellContractRepo;
        }

        public async Task InsertAsync(SellContractModel sell)
        {
            var rs = sell.MapProp<SellContractModel, SellContract>();
            await this._sellContractRepo.AddAsync(rs);
        }

        public async Task<SellContractModel> GetByIdAsync(string id)
        {
            var rs = await this._sellContractRepo.GetByIdAsync(id);
            return rs.MapProp<SellContract, SellContractModel>();
         
        }

        public async Task DeleteAsync(string id)
        {
            await this._sellContractRepo.DeleteAsync(id);

        }

        public async Task UpdateAsync(SellContractModel sell)
        {
            var rs = sell.MapProp<SellContractModel, SellContract>();
            await this._sellContractRepo.UpdateAsync(rs);
        }

        public Task ApproveAsync(SellContractModel buy)
        {
            throw new NotImplementedException();
        }
    }
}
