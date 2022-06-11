using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.BankPassbook;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Fcc;
using CertificatesOfDeposit.Models.Sell.Request;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Services.BankPassbook
{
    public interface IBankPassbookService
    {
        Task<BankPassbookModel> QuerySerialNumberAsync(string makerAcc, string prefix, string branchCode, string code);
        Task<string> PushSerialNumberAsync(string makerAcc, string prefix, string branchCode, string code, string serialNo);

        Task<BankPassbookModel> QuerySerialNumberByAccClass(string AccountClass, string MakerAcc, string BranchCode);
        Task<string> PushSerialNumberByAccClassAsync(string SerialNo, string AccountClass, string MakerAcc, string BranchCode);


    }
    public class BankPassbookService : IBankPassbookService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IBankPassbookRepository _bankPassbookRepo;
        private readonly IAccountClassFccRepository _accClassRepo;

        public BankPassbookService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IBankPassbookRepository accounTranRepo, IAccountClassFccRepository accountClassFccRepository)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._bankPassbookRepo = accounTranRepo;
            this._accClassRepo = accountClassFccRepository;
        }

        public async Task<string> PushSerialNumberAsync(string makerAcc, string prefix, string branchCode, string code, string serialNo)
        {
            var output = await _bankPassbookRepo.PushSerialNumber(makerAcc, prefix, branchCode, code, serialNo);
            return output;
        }

        public async Task<string> PushSerialNumberByAccClassAsync(string SerialNo,string AccountClass, string MakerAcc, string BranchCode)
        {
            var rsAccClass = await _accClassRepo.GetByIdAsync(AccountClass);
            if (rsAccClass != null)
            {
                var output = await _bankPassbookRepo.PushSerialNumber(MakerAcc, rsAccClass.MaKyHieu, BranchCode, rsAccClass.MaAnChi, SerialNo);
                return output;
            }
            return null;
        }

        public async Task<BankPassbookModel> QuerySerialNumberAsync(string makerAcc, string prefix, string branchCode, string code)
        {
            var output = await _bankPassbookRepo.GetSerialNumber(makerAcc, prefix, branchCode, code);
            return output.MapProp<BankPassbookErp, BankPassbookModel>();
        }

        public async Task<BankPassbookModel> QuerySerialNumberByAccClass(string AccountClass, string MakerAcc, string BranchCode)
        {
            var rsAccClass = await _accClassRepo.GetByIdAsync(AccountClass);
            if (rsAccClass != null)
            {
                var rsPassBook = await _bankPassbookRepo.GetSerialNumber(MakerAcc, rsAccClass.MaKyHieu, BranchCode, rsAccClass.MaAnChi);
                return rsPassBook.MapProp<BankPassbookErp, BankPassbookModel>();
            }
            return null;
        }
    }
}
