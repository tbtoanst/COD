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
    public interface IPartnerService
    {
        Task<List<PartnerModel>> GetAllAsync(string type);
    }
    public class PartnerService : IPartnerService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IPartnerRepository _partnerRepository;

        public PartnerService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IPartnerRepository partnerRepository)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._partnerRepository = partnerRepository;
        }

        public async Task<List<PartnerModel>> GetAllAsync(string type)
        {
            var rs = await this._partnerRepository.GetAllAsync(type);
            return rs.Select(s => s.MapProp<Partner, PartnerModel>()).ToList();
            
        }
    }
}
