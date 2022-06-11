using Aspose.Words;
using Aspose.Words.Reporting;
using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Buy;
using CertificatesOfDeposit.Models.Globally;
using CertificatesOfDeposit.Models.Sell;
using CertificatesOfDeposit.Models.Sell.Request;
using CertificatesOfDeposit.Services.Account;
using CertificatesOfDeposit.Services.SOA;
using CertificatesOfDeposit.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Controllers.Buy
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class BuyController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBuyService _buyService;
        private readonly IBuyContractService _buyContractService;
        private readonly ISellService _sellService;
        private readonly IAccountTranFccService _accountTranFccService;
        private readonly ISOAService _soaService;
        private readonly IBranchService _branchService;
        private readonly ITransactionService _transactionService;

        private string _productCode = AppConsts._productCode;
        private string _phaseCode = "NHAN_CHUYEN_NHUONG";
        private string _landCode_NVTV = "NVTV";
        private string _landCode_GDV = "GDV";
        private string _landCode_KSV = "KSV";

        public BuyController(IConfiguration configuration, ISellService sellService, IBuyService buyService, IBuyContractService buyContractService, ISOAService soaService,
            IAccountTranFccService accountTranFccService, IBranchService branchService, ITransactionService transactionService)
        {
            _config = configuration;
            _buyService = buyService;
            _buyContractService = buyContractService;
            _sellService = sellService;
            _accountTranFccService = accountTranFccService;
            _soaService = soaService;
            _branchService = branchService;
            _transactionService = transactionService;
        }



        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert([FromBody] RequestInsertBuyModel buy)
        {
            try
            {
                var buys = new List<BuyModel>();
                var bookId = Guid.NewGuid().ToString();
                var session = User.GetSession();

                var customerInfo = await _soaService.RetrieveCustomerRefDataMgmt(buy.BuyCif);
                var accountInfos = await _soaService.selectBookAccFromCIF(buy.BuyCif);
                var lstStatus = await _transactionService.GetStartStepAsync(_productCode, _landCode_NVTV, _phaseCode);

                foreach (var sellId in buy.SellIds)
                {
                    var sellData = await _sellService.GetByIdAsync(sellId);
                    var accInfos = await _accountTranFccService.QueryBuyAsync(sellData.AccountNum);
                    var accBuyInfo = accInfos.Single();
                    var buyData = buy.MapProp<RequestInsertBuyModel, BuyModel>();
                    buyData.Data = accBuyInfo.SerializeObject();
                    buyData.SellId = sellId;
                    buyData.BookingId = bookId;
                    buyData.CreatedUser = session.UserName;
                    buyData.CreatedDate = DateTime.Now;
                    buyData.TransactionDate = DateTime.Now;
                    buyData.CreatedBranchCode = session.BranchId;
                    buyData.BuyBranchCode = customerInfo.retrieveCustomerRefDataMgmt_out.TransactionInfo.BranchInfo.BranchCode;
                    buyData.BuyPhone = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Address.MobileNum;
                    buyData.BuyAddress = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Address.Address_vn;
                    buyData.BuyFullname = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Fullname_vn;
                    buyData.BuyIdNum = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.IDInfo.IDNum;
                    buyData.BuyPaymentBallance = accBuyInfo.GiaTriGiaoDich;
                    buyData.BuyPaymentFee = accBuyInfo.PhiChuyenNhuong;
                    buyData.BuyPaymentCCY = buy.BuyPaymentCCY;
                    if (buy.BuyPaymentMethod == "CK")
                    {
                        var accountBook = accountInfos.selectBookAcc_out.AccountInfo.First(f => f.AccountNum == buy.BuyPaymentAccountNo);
                        buyData.BuyAccountBalance = accountBook.AccountBalance;
                        buyData.BuyPaymentBranchCode = accountBook.AccountOpenBrandCode;
                        buyData.BranchAccountNo = buy.BranchAccountNo;
                    }

                    if(buy.BuyPayoutAccNum != null)
                    {
                        var accPayoutInfo = await _soaService.RetrieveCurrentAccountCASA(buy.BuyPayoutAccNum);
                        buyData.BuyPayoutBranchCode = accPayoutInfo.retrieveCurrentAccountCASA_out.AccountInfo.AccountOpenBrandCode;
                    }

                    foreach (var item in lstStatus)
                    {
                        switch (item.PhaseCode)
                        {
                            case "CHUYEN_NHUONG":
                                sellData.Status = item.ID;
                                sellData.UpdatedUser = session.UserName;
                                sellData.UpdatedDate = DateTime.Now;
                                await _sellService.UpdateAsync(sellData);
                                break;
                            case "NHAN_CHUYEN_NHUONG":
                                buyData.Status = item.ID;
                                break;
                            default:
                                break;
                        }
                    }

                    buys.Add(buyData);
                }

                await _buyService.InsertMultiAsync(buys);
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ErrorModel
                {
                    Code = "BadRequest",
                    Message = ex.Message
                });
            }

        }

        [HttpPatch]
        [Route("{buy_id}")]
        public async Task<IActionResult> Update([FromRoute(Name = "buy_id")] string buyId, [FromBody] RequestUpdateBuyModel buy)
        {
            var buyData = await _buyService.GetByIdAsync(buyId);
            // TODO kiểm tra thêm trạng thái
            if (buy.SeriNo != null)
                buyData.SeriNo = buy.SeriNo;
            if (buy.BuyPayoutAccNum != null)
                buyData.BuyPayoutAccNum = buy.BuyPayoutAccNum;
            var session = User.GetSession();
            buyData.UpdatedUser = session.UserName;
            buyData.UpdatedDate = DateTime.Now;

            await _buyService.UpdateAsync(buyData);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                
            });
        }

        [HttpPost]
        [Route("{buy_id}/approve")]
        public async Task<IActionResult> Approve([FromRoute(Name = "buy_id")] string buyId)
        {
            try
            {
                var buyData = await _buyService.GetByIdAsync(buyId);
                var session = User.GetSession();

                var lstStatus = await _transactionService.GetApproveStepAsync(buyData.Status, _productCode, _landCode_KSV);
                foreach (var item in lstStatus)
                {
                    switch (item.PhaseCode)
                    {
                        case "CHUYEN_NHUONG":
                            var sellData = await _sellService.GetByIdAsync(buyData.SellId);
                            sellData.Status = item.ID;
                            sellData.UpdatedUser = session.UserName;
                            sellData.UpdatedDate = DateTime.Now;
                            await _sellService.ApproveAsync(sellData);
                            break;
                        case "NHAN_CHUYEN_NHUONG":
                            buyData.Status = item.ID;
                            buyData.UpdatedUser = session.UserName;
                            buyData.UpdatedDate = DateTime.Now;
                            buyData.ApprovedUser = session.UserName;
                            buyData.ApprovedDate = DateTime.Now;
                            await _buyService.ApproveAsync(buyData);
                            break;
                        default:
                            break;
                    }
                }
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "ERROR",
                    Message = ex.Message
                });
            }
            
        }


        [HttpPost]
        [Route("{buy_id}/send")]
        public async Task<IActionResult> SendApprove([FromRoute(Name = "buy_id")] string buyId)
        {

            var buyData = await _buyService.GetByIdAsync(buyId);
            var session = User.GetSession();

            var lstStatus = await _transactionService.GetNextStepAsync(buyData.Status, _productCode, _landCode_GDV);
            foreach (var item in lstStatus)
            {
                switch (item.PhaseCode)
                {
                    case "CHUYEN_NHUONG":
                        var sellData = await _sellService.GetByIdAsync(buyData.SellId);
                        sellData.Status = item.ID;
                        sellData.UpdatedUser = session.UserName;
                        sellData.UpdatedDate = DateTime.Now;
                        await _sellService.UpdateAsync(sellData);
                        break;
                    case "NHAN_CHUYEN_NHUONG":
                        buyData.Status = item.ID;
                        buyData.UpdatedUser = session.UserName;
                        buyData.UpdatedDate = DateTime.Now;
                        buyData.TellerUser = session.UserName;
                        buyData.TellerDate = DateTime.Now;
                        await _buyService.UpdateAsync(buyData);
                        break;
                    default:
                        break;
                }
            }
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                
            });
        }

        [HttpPost]
        [Route("{buy_id}/reject")]
        public async Task<IActionResult> Reject([FromRoute(Name = "buy_id")] string buyId)
        {
            var buyData = await _buyService.GetByIdAsync(buyId);
            var session = User.GetSession();

            var lstStatus = await _transactionService.GetRejectStepAsync(buyData.Status, _productCode, _landCode_KSV);
            foreach (var item in lstStatus)
            {
                switch (item.PhaseCode)
                {
                    case "CHUYEN_NHUONG":
                        var sellData = await _sellService.GetByIdAsync(buyData.SellId);
                        sellData.Status = item.ID;
                        sellData.UpdatedUser = session.UserName;
                        sellData.UpdatedDate = DateTime.Now;
                        await _sellService.ApproveAsync(sellData);
                        break;
                    case "NHAN_CHUYEN_NHUONG":
                        buyData.Status = item.ID;
                        buyData.UpdatedUser = session.UserName;
                        buyData.UpdatedDate = DateTime.Now;
                        await _buyService.UpdateAsync(buyData);
                        break;
                    default:
                        break;
                }
            }
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                
            });
        }

        [HttpDelete]
        [Route("{buy_id}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "buy_id")] string buyId)
        {
            var buyData = await _buyService.GetByIdAsync(buyId);
            var session = User.GetSession();

            var lstStatus = await _transactionService.GetDeleteStepAsync(buyData.Status, _productCode, _landCode_GDV);
            foreach (var item in lstStatus)
            {
                switch (item.PhaseCode)
                {
                    case "CHUYEN_NHUONG":
                        var sellData = await _sellService.GetByIdAsync(buyData.SellId);
                        sellData.Status = item.ID;
                        sellData.UpdatedUser = session.UserName;
                        sellData.UpdatedDate = DateTime.Now;
                        await _sellService.ApproveAsync(sellData);
                        break;
                    case "NHAN_CHUYEN_NHUONG":
                        buyData.Status = item.ID;
                        buyData.UpdatedUser = session.UserName;
                        buyData.UpdatedDate = DateTime.Now;
                        buyData.DeletedUser = session.UserName;
                        buyData.DeletedDate = DateTime.Now;
                        await _buyService.UpdateAsync(buyData);
                        break;
                    default:
                        break;
                }
            }
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                
            });
        }

        [HttpGet]
        [Route("query")]
        public async Task<IActionResult> QueryAsync([FromQuery(Name = "account_class_code")] string accountClass,
                                                    [FromQuery(Name = "account_num")] string accountNum,
                                                    [FromQuery(Name = "cif")] string cifNum,
                                                    [FromQuery(Name = "status_code")] string status,
                                                    [FromQuery(Name = "trans_date")] DateTime? transactionDate,
                                                    [FromQuery(Name = "page_num")] int pageNum,
                                                    [FromQuery(Name = "page_size")] int pageSize)
        {
            var session = User.GetSession();
            var rs = await _buyService.QueryAsync(accountClass, accountNum, cifNum, status, session.BranchId, transactionDate, pageNum, pageSize);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("clean/query")]
        public async Task<IActionResult> CleanQueryAsync([FromQuery(Name = "page_num")] int pageNum,
                                                         [FromQuery(Name = "page_size")] int pageSize)
        {
            var rs = await _transactionService.CleanQueryBuyAsync(pageNum, pageSize);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("sell/query")]
        public async Task<IActionResult> QueryWithSellAsync([FromQuery(Name = "account_balance_from")] decimal? balanceFrom,
                                                    [FromQuery(Name = "account_balance_to")] decimal? balanceTo,
                                                    [FromQuery(Name = "remain_day_from")] decimal? remainDateFrom,
                                                    [FromQuery(Name = "remain_day_to")] decimal? remainDateTo,
                                                    [FromQuery(Name = "status_code")] string status,
                                                    [FromQuery(Name = "account_class")] string accountClass,
                                                    [FromQuery(Name = "account_no")] string accountNo,
                                                    [FromQuery(Name = "cif")] string cif,
                                                    [FromQuery(Name = "transaction_date")] DateTime? transactionDate,
                                                    [FromQuery(Name = "interest_rate")] decimal? interestRate,
                                                    [FromQuery(Name = "page_num")] int pageNum,
                                                    [FromQuery(Name = "page_size")] int pageSize)
        {
            var rs = await _buyService.QueryWithSellAsync(balanceFrom, balanceTo, remainDateFrom, remainDateTo, status, accountClass, cif, accountNo, transactionDate, null, interestRate, pageNum, pageSize);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("{buy_id}")]
        public async Task<IActionResult> GetBuyIdAsync([FromRoute(Name = "buy_id")] string buyId)
        {
            var rs = await _buyService.GetByIdAsync(buyId);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }


        [HttpPost]
        [Route("{buy_id}/contract/upload")]
        public async Task<IActionResult> UploadContract([FromRoute(Name = "buy_id")] string buyId, IFormFile files)
        {
            IFormFile file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var session = User.GetSession();
                var buy = new BuyContractModel();
                var fileName = $"{buyId}_HDBUY";
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    buy.BuyFile = memoryStream.ToArray();
                }
                buy.FileName = fileName;
                buy.FileType = file.ContentType;
                buy.CreatedUser = session.UserName;
                buy.CreatedAt = DateTime.Now;
                buy.BuyID = buyId;
                buy.Status = ContractConsts.CHO_DUYET;
                await _buyContractService.InsertAsync(buy);
            }

            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });

        }

        [HttpPost]
        [Route("{buy_id}/contract/reupload")]
        public async Task<IActionResult> ReuploadContract([FromRoute(Name = "buy_id")] string buyId, IFormFile files)
        {
            IFormFile file = Request.Form.Files[0];
            var rs = await _buyContractService.GetByIdAsync(buyId);

            var fileName = $"{buyId}_HDBUY";
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                rs.BuyFile = memoryStream.ToArray();
            }
            rs.FileName = fileName;
            rs.FileType = file.ContentType;

            rs.Status = ContractConsts.CHO_DUYET;
            var session = User.GetSession();
            rs.UpdatedUser = session.UserName;
            rs.UpdatedAt = DateTime.Now;
            await _buyContractService.UpdateAsync(rs);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("{buy_id}/contract/approve")]
        public async Task<IActionResult> ApproveContract([FromRoute(Name = "buy_id")] string buyId)
        {
            var rs = await _buyContractService.GetByIdAsync(buyId);
            rs.Status = ContractConsts.DA_DUYET;
            var session = User.GetSession();
            rs.ApprovedUser = session.UserName;
            rs.ApprovedAt = DateTime.Now;
            await _buyContractService.UpdateAsync(rs);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });

        }

        [HttpGet]
        [Route("{buy_id}/contract/download")]
        public async Task<IActionResult> GetContract([FromRoute(Name = "buy_id")] string buyId)
        {
            var rs = await _buyContractService.GetByIdAsync(buyId);
            return File(rs.BuyFile, rs.FileType, rs.FileName);
        }

        [HttpDelete]
        [Route("{buy_id}/contract")]
        public async Task<IActionResult> DeleteContract([FromRoute(Name = "buy_id")] string buyId)
        {
            await _buyContractService.DeleteAsync(buyId);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("{buy_id}/contract/preview")]
        public async Task<IActionResult> ContractBuyPreviewAsync([FromRoute(Name = "buy_id")] string buyId)
        {
            var rs = await _buyService.GetByIdAsync(buyId);
            var rsInfoCus = await _soaService.RetrieveCustomerRefDataMgmt(rs.BuyCif);
            var branchInfo = await _branchService.GetByBranchID(rs.CreatedBranchCode);
            var rsDetailAcc = await _soaService.RetrieveCurrentAccountCASA(rs.Sell.AccountNum);
            var output = rsInfoCus.retrieveCustomerRefDataMgmt_out;
            var cus = new CustomerModel
            {
                FullName = output.CustomerInfo?.Fullname,
                IdNo = output.CustomerInfo.IDInfo?.IDNum,
                IdDate = (DateTime.ParseExact(output.CustomerInfo.IDInfo?.IDIssuedDate, "yyyy-MM-dd hh:mm:ss.f", CultureInfo.CreateSpecificCulture("en-GB"))).ToString("dd/MM/yyyy"),
                IdLocation = output.CustomerInfo.IDInfo?.IDIssuedLocation,
                AddressThuongTru = output.CustomerInfo.Address?.Address_vn,
                AddressTamTru = output.CustomerInfo.Address?.Address1,
                PhoneNumber = output.CustomerInfo.Address?.MobileNum,
                Email = output.CustomerInfo.Address?.Email,
                FullNameTC = "",
                IdNoTC = "",
                IdDateTC = "",
                IdLocationTC = "",
                AddressThuongTruTC = "",
                AddressTamTruTC = "",
                PhoneNumberTC = "",
                EmailTC = "",
                FaxTC = ""
            };
            if (output.CIFInfo.CustomerType == "C")
            {
                cus = new CustomerModel
                {
                    FullName = "",
                    IdNo = "",
                    IdDate = "",
                    IdLocation = "",
                    AddressThuongTru = "",
                    AddressTamTru = "",
                    PhoneNumber = "",
                    Email = "",
                    FullNameTC = output.CustomerInfo?.Fullname,
                    IdNoTC = output.CustomerInfo.IDInfo?.IDNum,
                    IdDateTC = (DateTime.ParseExact(output.CustomerInfo.IDInfo?.IDIssuedDate, "yyyy-MM-dd hh:mm:ss.f", CultureInfo.CreateSpecificCulture("en-GB"))).ToString("dd/MM/yyyy"),
                    IdLocationTC = output.CustomerInfo.IDInfo?.IDIssuedLocation,
                    AddressThuongTruTC = output.CustomerInfo.Address?.Address_vn,
                    AddressTamTruTC = output.CustomerInfo.Address?.Address1,
                    PhoneNumberTC = output.CustomerInfo.Address?.MobileNum,
                    EmailTC = output.CustomerInfo.Address?.Email,
                    FaxTC = output.CustomerInfo.Address?.FaxNum
                };
            }
            var buyData = (rs.Data as object)?.MapProp<AccountTranFccModel>();
            OtherModel otherInfo = new OtherModel();
            switch (buyData.HinhThucLinhLai)
            {
                case "C":
                    otherInfo.HinhThucTraLai = "Cuối kỳ";
                    break;
                case "T":
                    otherInfo.HinhThucTraLai = "Trả trước";
                    break;
                case "D":
                    otherInfo.HinhThucTraLai = "Định kỳ";
                    break;
            }
            otherInfo.SoDuVn = buyData.SoDu.ToString("#,##0");
            otherInfo.GiaTriGiaoDich = buyData.GiaTriGiaoDich.ToString("#,##0");
            otherInfo.NgayMo = buyData.NgayMo.ToString("dd/MM/yyyy");
            otherInfo.NgayTaiKy = buyData.NgayTaiKy.ToString("dd/MM/yyyy");
            otherInfo.LaiSuat = buyData.LaiSuat.ToString();
            otherInfo.SoTienCn = buyData.SoTienKhNhanCn.ToString("#,##0");
            otherInfo.SoTienThuePhi = buyData.SoTienThuPhi.ToString("#,##0");
            otherInfo.SoTienCnTxt = Converter.NumberToTextVN(buyData.SoTienKhNhanCn);
            otherInfo.BranchName = branchInfo.Name;
            otherInfo.NguoiDaiDien = buyData.NguoiDaiDien;
            var rsPayout = await _soaService.RetrieveCurrentAccountCASA(rs.BuyPayoutAccNum);
            var rsPayouts = await _soaService.RetrieveCurrentAccountCASA(rs.BuyPaymentAccountNo);
            if (rsPayout.retrieveCurrentAccountCASA_out.AccountInfo?.AccountOpenBrandCode != null)
            {
                var branchInfos = await _branchService.GetByBranchID(rsPayout.retrieveCurrentAccountCASA_out.AccountInfo.AccountOpenBrandCode);
                otherInfo.BranchNameAcc = branchInfos.Name;

            }
            if (rsPayouts.retrieveCurrentAccountCASA_out.AccountInfo?.AccountOpenBrandCode != null)
            {
                var branchInfoss = await _branchService.GetByBranchID(rsPayouts.retrieveCurrentAccountCASA_out.AccountInfo?.AccountOpenBrandCode);
                otherInfo.BranchNameAccOut = branchInfoss.Name;
            }
            ReportModel rsData = new ReportModel
            {
                CustomerInfo = cus,
                DataInfo = buyData,
                BuyInfo = rs,
                OtherInfo = otherInfo
            };
            string fileName = "";
            if (rs.BuyPaymentMethod == "CK")
            {
                fileName = "HD_MUA_CHUYENKHOAN.doc";
            }
            else
            {
                fileName = "HD_MUA_TIENMAT.doc";
            }
            string fileDir = _config["DirectoryConfig:word"];
            string rootdir = Directory.GetCurrentDirectory();
            Document doc = new Document(rootdir + fileDir + fileName);
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, rsData, "rsData");
            MemoryStream stream = new MemoryStream();
            doc.Save(stream, Aspose.Words.SaveFormat.Doc);
            byte[] pdfBytes = stream.ToArray();
            if (pdfBytes == null)
                return Content("Container doesn't exists");
            return File(pdfBytes, "application/doc",
                "HD_NHANCN_" + output.CustomerInfo?.Fullname + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".doc");
        }
    }


}
