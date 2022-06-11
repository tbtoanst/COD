using Dapper;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificatesOfDeposit.API.Models.Account;

namespace CertificatesOfDeposit.Services.Account
{
    public interface IBranchService
    {
        /// <summary>
        /// Lấy danh sách thông tin đơn vị
        /// </summary>
        /// <returns></returns>
        Task<BranchModel> GetByBranchID(string ID);
        Task<BranchModel> GetByBranchCode(string branchCode);
        Task<IReadOnlyList<BranchModel>> GetAllBranch();
    }

    public class BranchService : IBranchService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IBranchRepository _branchRepo;

        public BranchService(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IBranchRepository branchRepo)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._branchRepo = branchRepo;
        }

        public async Task<IReadOnlyList<BranchModel>> GetAllBranch()
        {
            var branchs = await this._branchRepo.GetAllAsync();
            return branchs.Select(s => s.MapProp<Branch, BranchModel>()).ToList();
        }

        public async Task<BranchModel> GetByBranchCode(string branchCode)
        {
            var branch = await this._branchRepo.GetByBranchCodeAsync(branchCode);
            return branch.MapProp<Branch, BranchModel>();
        }

        public async Task<BranchModel> GetByBranchID(string ID)
        {
            var branch = await this._branchRepo.GetByIdAsync(ID);
            return branch.MapProp<Branch, BranchModel>();
        }
    }
}
