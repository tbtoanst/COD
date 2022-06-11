using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Account.Request;
using CertificatesOfDeposit.Models.Globally;
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

namespace CertificatesOfDeposit.Controllers.Employee
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ISOAService _SOAService;

        public EmployeeController(ISOAService soaService)
        {
            _SOAService = soaService;
        }

        [HttpPost]
        [Route("Get_Emp_List_Seller")]
        public async Task<IActionResult> GetListSellerInBranch([FromBody] RequestEmployeeBySOA employee)
        {
            var strEmployeeClass = employee.EmployeeClass?.Trim().ToString().ToUpper();
            var strBranchID = employee.BranchCode?.Trim().ToString().ToUpper();

            var employeeInfo = await _SOAService.SelectDirectSellerTypeOfBranch(strEmployeeClass, strBranchID);
            if (employeeInfo != null)
            {
                var transInfo = employeeInfo.SelectCustomerRefDataMgmtCIFNum_out.TransactionInfo;
                switch (transInfo.TransactionReturn)
                {
                    case 1:
                        return Ok(new ErrorModel
                        {
                            Code = "OK",
                            Message = "Successful!",
                            Data = new
                            {
                                employeeInfo.SelectCustomerRefDataMgmtCIFNum_out.HrEmpDataInfo,
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
    }
}
