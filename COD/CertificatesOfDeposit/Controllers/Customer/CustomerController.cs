using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Account.Request;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Models.SOA;
using CertificatesOfDeposit.Services.Account;
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

namespace CertificatesOfDeposit.Controllers.Customer
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ISOAService _SOAService;
        private readonly IAccountTranFccService _AccountTranFccService;

        public CustomerController(ISOAService soaService, IAccountTranFccService accountTranFccService)
        {
            _SOAService = soaService;
            _AccountTranFccService = accountTranFccService;
        }

        [HttpGet]
        [Route("{cif}/Get_Cust_Info_Core")]
        public async Task<IActionResult> GetCustomerInfoInCore([FromRoute(Name = "cif")] string cif)
        {
            var strCIF = cif?.ToString().Trim().ToUpper();

            var customerInfo = await _SOAService.RetrieveCustomerRefDataMgmt(strCIF);
            if (customerInfo != null)
            {
                var transInfo = customerInfo.retrieveCustomerRefDataMgmt_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = new
                            {
                                customerInfo.retrieveCustomerRefDataMgmt_out.CIFInfo,
                                customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo
                            }

                        });
                    default:
                        return NotFound(new ErrorModel
                        {
                            Code = "CORE_BANKING_NOT_FOUND",
                            Message = $"NOT_FOUND: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return NotFound(new ErrorModel
            {
                Code = "CORE_BANKING_NOT_FOUND",
                Message = "NOT_FOUND"
            });
        }

        [HttpPost]
        [Route("Get_Cust_Info_Core_MultiInput")]
        public async Task<IActionResult> GetCustomerInfoInCoreByMultiInput([FromBody] RequestCustomerBySOA customer)
        {
            var strCIF = customer.CIF?.Trim().ToString().ToUpper();
            var strIDNumber = customer.IDNumber?.Trim().ToString().ToUpper();
            var strPhoneNumber = customer.PhoneNumber?.Trim().ToString().ToUpper();

            var customerInfo = await _SOAService.SelectCustomerRefDataMgmtCIFNum(strCIF, strIDNumber, strPhoneNumber);
            if (customerInfo != null)
            {
                var transInfo = customerInfo.SelectCustomerRefDataMgmtCIFNum_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = new
                            {
                                customerInfo.SelectCustomerRefDataMgmtCIFNum_out.CustomerList,
                            }

                        });
                    default:
                        return NotFound(new ErrorModel
                        {
                            Code = "CORE_BANKING_NOT_FOUND",
                            Message = $"NOT_FOUND: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return NotFound(new ErrorModel
            {
                Code = "CORE_BANKING_NOT_FOUND",
                Message = "NOT_FOUND"
            });
        }

        [HttpGet]
        [Route("{cif}/Get_Cust_Payment_Account")]
        public async Task<IActionResult> GetPaymentAccount([FromRoute(Name = "cif")] string cif)
        {
            var employeeInfo = await _SOAService.selectBookAccFromCIF(cif);
            var accDetail = await _AccountTranFccService.GetAccountDetailsAsync(cif);
            List<AccountInfoModel> lstAccountInfo = new List<AccountInfoModel>();
            foreach (var acc in employeeInfo.selectBookAcc_out.AccountInfo)
            {
                var accountDetail = accDetail.Where(a => a.custAcNo == acc.AccountNum).ToList();
                if (accountDetail.Count == 1)
                {
                    acc.AccountStatusActive = accountDetail.FirstOrDefault().trangThai;
                }
                lstAccountInfo.Add(acc);
            };
            if (employeeInfo != null)
            {
                var transInfo = employeeInfo.selectBookAcc_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = new
                            {
                                accountInfo = lstAccountInfo
                                                    //.Where(a=>a.AccountStatusActive.ToUpper() != "FROZEN")
                                                    .ToArray()
                            }

                        });
                    default:
                        return NotFound(new ErrorModel
                        {
                            Code = "CORE_BANKING_NOT_FOUND",
                            Message = $"NOT_FOUND: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return NotFound(new ErrorModel
            {
                Code = "CORE_BANKING_NOT_FOUND",
                Message = "NOT_FOUND"
            });
        }

        [HttpGet]
        [Route("{acc_num}/Get_Payment_Account_Info")]
        public async Task<IActionResult> GetPaymentAccountInfo([FromRoute(Name = "acc_num")] string accNum)
        {
            var accInfo = await _SOAService.RetrieveCurrentAccountCASA(accNum);
            if (accInfo != null)
            {
                var transInfo = accInfo.retrieveCurrentAccountCASA_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = accInfo.retrieveCurrentAccountCASA_out.AccountInfo

                        });
                    default:
                        return NotFound(new ErrorModel
                        {
                            Code = "FAIL",
                            Message = $"NOT_FOUND: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return NotFound(new ErrorModel
            {
                Code = "FAIL",
                Message = "NOT_FOUND"
            });
        }

        [HttpGet]
        [Route("{branch_code}/Get_Payment_Account_form_branch_code")]
        public async Task<IActionResult> GetPaymentAccountFormBranchInfo([FromRoute(Name = "branch_code")] string branchCode)
        {
            var accInfo = await _SOAService.SelectCurrentAccountFromCIF(branchCode);
            if (accInfo != null)
            {
                var transInfo = accInfo.SelectCurrentAccountFromCIF_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = accInfo.SelectCurrentAccountFromCIF_out.AccountInfo.Where(a => a.AccountClassCode.ToUpper().StartsWith("TR"))

                        });
                    default:
                        return NotFound(new ErrorModel
                        {
                            Code = "FAIL",
                            Message = $"NOT_FOUND: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return NotFound(new ErrorModel
            {
                Code = "FAIL",
                Message = "NOT_FOUND"
            });
        }
    }
}
