using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Models.Sell.Request;
using CertificatesOfDeposit.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Controllers.Account
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBranchService _branchService;
        public BranchController(IConfiguration configuration, IBranchService branchService)
        {
            _config = configuration;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var rs = await _branchService.GetAllBranch();
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("{branch_id}")]
        public async Task<IActionResult> GetBranch([FromRoute(Name = "branch_id")] string id)
        {
            var strID = id?.Trim().ToUpper();
            var rs = await _branchService.GetByBranchID(strID);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }
    }
}
