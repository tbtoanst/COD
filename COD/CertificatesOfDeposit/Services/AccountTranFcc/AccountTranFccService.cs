using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.AccountTranFcc;
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
    public interface IAccountTranFccService
    {
        Task<List<AccountDetailCASAModel>> GetAccountDetailsAsync(string cif);
        Task<List<AccountTranFccModel>> QueryAsync(string cif);
        Task<List<AccountTranFccModel>> QueryBuyAsync(string cif);
    }
    public class AccountTranFccService : IAccountTranFccService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IAccountTranFccRepository _accounTranRepo;
        private readonly IJointHolderRepository _jointHolderRepo;

        public AccountTranFccService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IAccountTranFccRepository accounTranRepo, IJointHolderRepository jointHolderRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._accounTranRepo = accounTranRepo;
            this._jointHolderRepo = jointHolderRepo;
        }

        

        public async Task<List<AccountTranFccModel>> QueryAsync(string cif)
        {
            var rs = await this._accounTranRepo.GetAllAsync(cif);
            return rs.Select(s =>
            {
                var jointHolders = this._jointHolderRepo.GetAllAsync(s.Stk).Result;
                var data = s.MapProp<AccountTranFcc, AccountTranFccModel>();// jointHolders.ToList();
                data.DSDongChuSoHuu = jointHolders.Select(s=>s.MapProp<JointHolder, JointHolderModel>()).ToList();
                return data;
            }).ToList();
        }
        public async Task<List<AccountTranFccModel>> QueryBuyAsync(string cif)
        {
            var rs = await this._accounTranRepo.GetBuyAllAsync(cif);
            return rs.Select(s =>
            {
                var jointHolders = this._jointHolderRepo.GetAllAsync(s.Stk).Result;
                var data = s.MapProp<AccountTranFcc, AccountTranFccModel>();// jointHolders.ToList();
                data.DSDongChuSoHuu = jointHolders.Select(s => s.MapProp<JointHolder, JointHolderModel>()).ToList();
                return data;
            }).ToList();
        }

        public async Task<List<AccountDetailCASAModel>> GetAccountDetailsAsync(string cif)
        {
            var rs = await this._accounTranRepo.GetAccountDetailsAsync(cif);
            var ouput = rs.Select(s => s.MapProp<AccountDetailCASA, AccountDetailCASAModel>())
                          //.Where(s => s.trangThai.ToUpper() != "FROZEN")
                          .ToList();
            return ouput;
        }
    }
}
