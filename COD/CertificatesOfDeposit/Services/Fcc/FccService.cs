using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Fcc;
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
    public interface IFccService
    {
        Task<List<JointHolderModel>> GetAllAsync(string accNum);
    }
    public class FccService : IFccService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IJointHolderRepository _jointHolderRepo;

        public FccService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IJointHolderRepository jointHolderRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._jointHolderRepo = jointHolderRepo;
        }

        public async Task<List<JointHolderModel>> GetAllAsync(string accNum)
        {
            var rs = await this._jointHolderRepo.GetAllAsync(accNum);
            return rs.Select(s => s.MapProp<JointHolder, JointHolderModel>()).ToList();
            
        }
    }
}
