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

namespace CertificatesOfDeposit.Controllers.Account
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRoleService _roleService;
        private readonly ISOAService _SOAService;
        private readonly IBranchService _branchService;

        public AccountController(IConfiguration configuration, IRoleService roleService, ISOAService soaService, IBranchService branchService)
        {
            _config = configuration;
            _roleService = roleService;
            _SOAService = soaService;
            _branchService = branchService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] RequestLoginModel user)
        {
            var userName = user.UserName?.ToString().ToLower();
            var password = user.Password?.ToString();
            var roles = await _roleService.GetRoleAsync(userName, password, AppConsts._productCode);
            string branchCodeFCC = "", branchNameFCC = "";
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    var isCheckCore = await _roleService.IsRequiredCheckExistsFcc(role);
                    if (isCheckCore)
                    {
                        var infoUserCore = await _SOAService.RetrieveCoreBankAccount(userName);                
                        branchCodeFCC = infoUserCore.RetrieveCoreBankAccount_out.CoreBankAccount.BranchInfo.BranchCode;
                        branchNameFCC = null;
                        if (!string.IsNullOrEmpty(branchCodeFCC))
                        {
                            var infoBranchFCC = await _branchService.GetByBranchID(branchCodeFCC);
                            branchNameFCC = infoBranchFCC.Name;
                        }
                        var transInfo = infoUserCore.RetrieveCoreBankAccount_out.TransactionInfo;
                        if (transInfo!= null)
                        {
                            switch (transInfo.TransactionErrorCode)
                            {
                                case "00":
                                    break;
                                case "01":
                                    return NotFound(new { code = "LOGIN_BADREQUEST", message = $"Người dùng không có quyền truy cập CoreFCC (Thuộc nhóm quyền cần có user FCC)." });
                            }
                        }
                    }
                }
            }
            else
            {
                if (!roles.Any())
                {
                    return NotFound(new { code = "LOGIN_BADREQUEST", message = $"Người dùng không có quyền truy cập (Trên hệ thống Phân quyền tập trung)." });
                }
            }

            var userInfo = await _roleService.GetUserInfoAsync(userName.ToLower(), password);
            if (userInfo != null)
            {
                var position = await _roleService.GetPostionAsync(userInfo.Name);
                if (position == null)
                {
                    return NotFound(new { code = "LOGIN_BADREQUEST", message = $"Người dùng chưa đăng ký tài khoản." });
                }
                if (position.Department.Id == null)
                {
                    return NotFound(new { code = "LOGIN_BADREQUEST", message = $"Người dùng chưa đăng ký phòng ban." });
                }
                if (position.Branch.Code == null)
                {
                    return NotFound(new { code = "LOGIN_BADREQUEST", message = $"Người dùng chưa đăng ký chi nhánh." });
                }

                var menus = await _roleService.GetMenusAsync(roles);

                var dept = position.Department;
                var branch = position.Branch;

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:JwtBearer:SecurityKey"]));
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userInfo.Id),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Name),
                    new Claim(ClaimTypes.Name, userInfo.FullName),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypesCustom.DEPARTMENT_ID, dept.Id),
                    new Claim(ClaimTypesCustom.DEPARTMENT_CODE, dept.Code),
                    new Claim(ClaimTypesCustom.DEPARTMENT_NAME, dept.Name),
                    new Claim(ClaimTypesCustom.BRANCH_ID, branch.Id),
                    new Claim(ClaimTypesCustom.BRANCH_CODE, branch.Id),
                    new Claim(ClaimTypesCustom.BRANCH_NAME, branch.Name),
                    new Claim(ClaimTypesCustom.BRANCH_CODE_FCC, branchCodeFCC),
                    new Claim(ClaimTypesCustom.BRANCH_NAME_FCC, branchNameFCC),
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _config["Authentication:JwtBearer:Issuer"],
                    audience: _config["Authentication:JwtBearer:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(500),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new ErrorModel
                {
                    Code = "LOGIN_OK",
                    Message = "Successful!",
                    Data = new
                    {
                        token = tokenString,
                        user_info = new
                        {
                            id = userInfo.Id,
                            username = userInfo.Name,
                            fullname = userInfo.FullName,
                            email = userInfo.Email,
                            role = roles,
                            positions = position,
                            positions_fcc = new
                            {
                                branch_code = branchCodeFCC,
                                branch_name = branchNameFCC,
                            }
                        },
                        menus
                    }
                });
            }

            return Unauthorized(new ErrorModel
            {
                Code = "LOGIN_UNAUTHORIZED",
                Message = "UNAUTHORIZED"
            });
        }

        [HttpPost]
        [Route("Verify_User_Core_Banking")]
        public async Task<IActionResult> VerifyUserCoreBanking([FromBody] RequestLoginModel user)
        {
            var userName = user.UserName?.Trim().ToString().ToUpper();
            var password = user.Password?.Trim().ToString();

            var userInfoCore = await _SOAService.CheckUserCoreBankingAsync(userName, password);
            if (userInfoCore != null)
            {
                var transInfo = userInfoCore.checkAccountCoreBank_out.TransactionInfo;
                switch (transInfo.TransactionErrorCode)
                {
                    case "00":
                        return Ok(new ErrorModel
                        {
                            Code = "CHECK_CORE_BANKING_OK",
                            Message = "Successful!",

                        });
                    default:
                        return Unauthorized(new ErrorModel
                        {
                            Code = "CHECK_CORE_BANKING_UNAUTHORIZED",
                            Message = $"UNAUTHORIZED: {transInfo.TransactionErrorCode} - {transInfo.TransactionErrorMsg}"
                        });
                }
            }
            return Unauthorized(new ErrorModel
            {
                Code = "CHECK_CORE_BANKING_UNAUTHORIZED",
                Message = "UNAUTHORIZED"
            });
        }

        [HttpGet]
        [Route("{refreshToken}/refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            return Unauthorized();
        }
    }
}
