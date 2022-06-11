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
    public class PartnerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPartnerService _partnerService;
        public PartnerController(IConfiguration configuration, IPartnerService partnerService)
        {
            _config = configuration;
            _partnerService = partnerService;
        }

        [HttpGet]
        [Route("{type}")]
        public async Task<IActionResult> QueryAsync([FromRoute(Name = "type")] string type)
        {
            var rs = await _partnerService.GetAllAsync(type);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

    }
}
