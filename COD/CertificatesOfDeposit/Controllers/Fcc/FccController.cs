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

namespace CertificatesOfDeposit.Controllers.Fcc
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class FccController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountTranFccService _accountTranService;
        private readonly IAccountClassFccService _accountClassService;
        private readonly IFccService _fccService;
        public FccController(IConfiguration configuration, IAccountTranFccService accountTranService, IAccountClassFccService accountClassService, IFccService fccService)
        {
            _config = configuration;
            _accountTranService = accountTranService;
            _accountClassService = accountClassService;
            _fccService = fccService;
        }

        [HttpGet]
        [Route("{cif}/get_account_list_fcc")]
        public async Task<IActionResult> QuerysAsync([FromRoute(Name = "cif")] string cif)
        {
            var rs = await _accountTranService.QueryAsync(cif);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("{accClass}/get_account_class_fcc")]
        public async Task<IActionResult> QueryAccountClassAsync([FromRoute(Name = "accClass")] string accClass)
        {
            var strAccClass = accClass?.Trim().ToString().ToUpper();
            var rs = await _accountClassService.QueryAsync(strAccClass);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("get_list_account_class_fcc")]
        public async Task<IActionResult> QueryListAccountClassAsync()
        {
            var rs = await _accountClassService.GetAllAsync();
            var ouput = rs.Select(c => new
            {
                ma_sp = c.MaSp,
                ten_sp = c.TenSp
            }).ToList();
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = ouput
            });
        }

    }
}
