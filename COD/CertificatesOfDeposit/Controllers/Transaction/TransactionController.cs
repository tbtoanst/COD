using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Account.Request;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Models.Transaction;
using CertificatesOfDeposit.Services.Account;
using CertificatesOfDeposit.Services.SOA;
using CertificatesOfDeposit.Services.Transactions;
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

namespace CertificatesOfDeposit.Controllers.Transaction
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITransactionService _transactionService;
        public TransactionController(IConfiguration configuration, ITransactionService transactionService)
        {
            _config = configuration;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionList([FromQuery] RequestGetListTransactionModel transaction)
        {
            var result = await _transactionService.GetListAsync(transaction.BranchCode, transaction.FromDate, transaction.toDate, transaction.ContractNo, transaction.Status, transaction.PageNum, transaction.PageSize, transaction.AccountNum);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = result
            });
        }

        [HttpPatch]
        [Route("{tran_id}/approve")]
        public async Task<IActionResult> Approve([FromRoute(Name = "tran_id")] string tranId)
        {
            await _transactionService.Update(tranId);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpGet]
        [Route("lock")]
        public async Task<IActionResult> GetLockTran()
        {
           var status = await _transactionService.GetStatusLockAsync();
            return Ok(new ErrorModel
            {
                Code = "OK",
                Data = status,
                Message = "Successful!"
            });
        }

        /// <summary>
        /// Cập nhật lock cuối ngày
        /// </summary>
        /// <param name="lockId">ID cho lock_id = TKGTCG</param>
        /// <param name="requestTransactionLock"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("lock/{lock_id}")]
        public async Task<IActionResult> UpdateTransactionLock([FromRoute(Name = "lock_id")] string lockId, [FromBody] RequestTransactionLockModel requestTransactionLock)
        {
            var session = User.GetSession();
            var status = await _transactionService.UpdateTransactionLock(lockId, session.UserName, requestTransactionLock.Status);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Data = status,
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("excecute_end_day")]
        public async Task<IActionResult> ExcecuteCleanEndDayAsync()
        {
            var session = User.GetSession();
            var status = await _transactionService.ExcecuteCleanEndDayAsync(session.UserName);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Data = status,
                Message = "Successful!"
            });
        }
    }
}
