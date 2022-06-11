using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Account.Request;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Services.Account;
using CertificatesOfDeposit.Services.BankPassbook;
using CertificatesOfDeposit.Services.SOA;
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

namespace CertificatesOfDeposit.Controllers.BankPassbook
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class BankPassbookController : ControllerBase
    {
        private readonly ISOAService _SOAService;
        private readonly IBankPassbookService _bankPassbookService;
        public BankPassbookController(ISOAService soaService, IBankPassbookService bankPassbookService)
        {
            _SOAService = soaService;
            _bankPassbookService = bankPassbookService;
        }

        //[HttpPost]
        //[Route("get_bank_passbook_info_soa")]
        //public async Task<IActionResult> GetBankPassbookInfo_By_SOA([FromBody] RequestBankPassbookInfo passbook)
        //{
        //    var session = User.GetSession();
        //    var Prefix = passbook.Prefix?.Trim().ToString().ToUpper();
        //    var Code = passbook.Code?.Trim().ToString().ToUpper();
        //    var BranchCode = session.BranchCode?.Trim().ToString().ToUpper();
        //    var MakerAcc = session.UserName?.Trim().ToString().ToUpper();

        //    var employeeInfo = await _SOAService.RetreiveSerialNumber(Prefix, Code, BranchCode, MakerAcc);
        //    if (employeeInfo != null)
        //    {
        //        return Ok(new ErrorModel
        //        {
        //            Code = "CHECK_CORE_BANKING_OK",
        //            Message = "Successful!",
        //            Data = new
        //            {
        //                employeeInfo.RetreiveSerialNumber_out.ErpInfo,
        //            }

        //        });
        //    }
        //    return NotFound(new ErrorModel
        //    {
        //        Code = "CORE_BANKING_NOT_FOUND",
        //        Message = "NOT_FOUND"
        //    });
        //}

        [HttpGet]
        [Route("{acc_class}/get_bank_passbook_info")]
        public async Task<IActionResult> GetBankPassbookInfo([FromRoute(Name = "acc_class")] string accountClass)
        {
            var session = User.GetSession();
            var AccountClass = accountClass?.Trim().ToString().ToUpper();
            var BranchCode = session.BranchId?.Trim().ToString().ToUpper();
            var MakerAcc = session.UserName?.Trim().ToString().ToUpper();

            var passbookInfo = await _bankPassbookService.QuerySerialNumberByAccClass(AccountClass, MakerAcc, BranchCode);
            if (passbookInfo != null)
            {
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                    Data = passbookInfo
                });
            }
            return NotFound(new ErrorModel
            {
                Code = "ERROR",
                Message = "NOT_FOUND"
            });
        }

        [HttpPost]
        [Route("push_bank_passbook_serial_no")]
        public async Task<IActionResult> PushBankPassbookSerialNo([FromBody] RequestBankPassbookPushSerialNo passbook)
        {
            var session = User.GetSession();
            var strSerialNo = passbook.SerialNumber?.Trim().ToString().ToUpper();
            var strAccountClass = passbook.AccountClass?.Trim().ToString().ToUpper();
            var BranchCode = session.BranchId?.Trim().ToString().ToUpper();
            var MakerAcc = session.UserName?.Trim().ToString().ToUpper();

            var passbookInfo = await _bankPassbookService.PushSerialNumberByAccClassAsync(strSerialNo, strAccountClass, MakerAcc, BranchCode);
            if (passbookInfo != null)
            {
                switch (passbookInfo)
                {
                    case "Y":
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = new
                            {
                                status = passbookInfo
                            }
                        });
                    case "N":
                        return NotFound(new ErrorModel
                        {
                            Code = "FAIL",
                            Message = "Push fail!",
                            Data = new
                            {
                                status = passbookInfo
                            }
                        });
                    default:
                        break;
                }
            }
            return BadRequest(new ErrorModel
            {
                Code = "ERROR",
                Message = "NOT_FOUND"
            });
        }
    }
}
