using CertificatesOfDeposit.Consts;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.Sell;
using CertificatesOfDeposit.Models.Globally;
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
using CertificatesOfDeposit.Models.Sell.Request;
using Microsoft.AspNetCore.Http;
using System.IO;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Services.SOA;
using CertificatesOfDeposit.Models.Buy;
using Aspose.Words;
using Aspose.Words.Reporting;
using System.Globalization;
using CertificatesOfDeposit.Services.Transactions;

namespace CertificatesOfDeposit.Controllers.Account
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class SellController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISellService _sellService;
        private readonly IBuyService _buyService;
        private readonly ISellContractService _sellContractService;
        private readonly IAccountTranFccService _accountTranFccService;
        private readonly IJointHolderRepository _jointHolderRepo;
        private readonly ISOAService _soaService;
        private readonly IBranchService _branchService;
        private readonly ITransactionService _transactionService;

        private string _productCode = AppConsts._productCode;
        private string _phaseCode = "CHUYEN_NHUONG";
        private string _landCode_NVTV = "NVTV";
        private string _landCode_GDV = "GDV";
        private string _landCode_KSV = "KSV";

        public SellController(IConfiguration configuration, ISellService sellService, ISellContractService sellContractService, IAccountTranFccService accountTranFccService, IJointHolderRepository jointHolderRepo, ISOAService soaService, IBranchService branchService, ITransactionService transactionService, IBuyService buyService)
        {
            _config = configuration;
            _sellService = sellService;
            _buyService = buyService;
            _sellContractService = sellContractService;
            _accountTranFccService = accountTranFccService;
            _soaService = soaService;
            _branchService = branchService;
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] RequestInsertSellModel sell)
        {
            try
            {
                var sells = new List<SellModel>();
                var bookingId = Guid.NewGuid().ToString();
                var session = User.GetSession();
                var customerInfo = await _soaService.RetrieveCustomerRefDataMgmt(sell.SellCif);
                var lstStatus = await _transactionService.GetStartStepAsync(_productCode, _landCode_NVTV, _phaseCode);
                foreach (var accNum in sell.AccountNumList)
                {
                    var accountInfos = await _soaService.selectBookAccFromCIF(sell.SellCif);

                    var accInfos = await _accountTranFccService.QueryAsync(accNum);
                    var accInfo = accInfos.Single();
                    var sellData = sell.MapProp<RequestInsertSellModel, SellModel>();
                    sellData.Data = accInfo.SerializeObject();
                    sellData.BookingId = bookingId;
                    sellData.AccountNum = accInfo.Stk;
                    sellData.AccountBalance = accInfo.SoDu;
                    sellData.AccountClass = accInfo.LoaiHinh;
                    sellData.RemainDay = accInfo.SoNgayConLai;
                    sellData.SellPaymentBallance = accInfo.GiaTriGiaoDich;
                    sellData.SellPaymentFee = accInfo.PhiChuyenNhuong;
                    sellData.TransactionDate = DateTime.Now;
                    sellData.CreatedUser = session.UserName;
                    sellData.CreatedDate = DateTime.Now;
                    sellData.CreatedBranchCode = session.BranchId;

                    sellData.SellBranchCode = customerInfo.retrieveCustomerRefDataMgmt_out.TransactionInfo.BranchInfo.BranchCode;
                    sellData.SellPhone = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Address.MobileNum;
                    sellData.SellAddress = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Address.Address_vn;
                    sellData.SellFullname = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.Fullname_vn;
                    sellData.SellIdNum = customerInfo.retrieveCustomerRefDataMgmt_out.CustomerInfo.IDInfo.IDNum;
                    sellData.SellPaymentCCY = sell.SellPaymentCCY;

                    if (sell.SellPaymentMethod == "CK")
                    {
                        var accountBook = accountInfos.selectBookAcc_out.AccountInfo.First(f => f.AccountNum == sell.SellPaymentAccountNo);
                        sellData.SellPaymentBranchCode = accountBook.AccountOpenBrandCode;
                        sellData.BranchAccountNo = sell.BranchAccountNo;
                    }
                    
                    foreach (var item in lstStatus)
                    {
                        switch (item.PhaseCode)
                        {
                            case "CHUYEN_NHUONG":
                                sellData.Status = item.ID;
                                break;
                            case "NHAN_CHUYEN_NHUONG":
                                break;
                            default:
                                break;
                        }
                    }

                    sells.Add(sellData);
                }
                await _sellService.InsertMultiAsync(sells);
                return Ok(new ErrorModel
                {
                    Code = "OK",
                    Message = "Successful!",
                    
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
        [Route("{sell_id}")]
        public async Task<IActionResult> Update([FromRoute(Name = "sell_id")] string sellId, [FromBody] RequestSellModel sell)
        {
            var sellData = await _sellService.GetByIdAsync(sellId);
            //TODO kiểm tra thêm trạng thái
            var session = User.GetSession();
            sellData.UpdatedUser = session.UserName;
            sellData.UpdatedDate = DateTime.Now;

            await _sellService.UpdateAsync(sellData);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                
            });
        }

        [HttpDelete]
        [Route("{sell_id}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "sell_id")] string sellId)
        {
            var session = User.GetSession();
            var sellData = await _sellService.GetByIdAsync(sellId);
            var lstStatus = await _transactionService.GetDeleteStepAsync(sellData.Status, _productCode, _landCode_GDV);
            foreach (var item in lstStatus)
            {
                switch (item.PhaseCode)
                {
                    case "CHUYEN_NHUONG":
                        sellData.Status = item.ID;
                        sellData.UpdatedUser = session.UserName;
                        sellData.UpdatedDate = DateTime.Now;
                        sellData.DeletedUser = session.UserName;
                        sellData.DeletedDate = DateTime.Now;
                        await _sellService.UpdateAsync(sellData);
                        break;
                    case "NHAN_CHUYEN_NHUONG":
                        var buyData = await _buyService.GetLastBySellIdAsync(sellId);
                        buyData.Status = item.ID;
                        await _buyService.UpdateAsync(buyData);
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

        [HttpPost]
        [Route("{sell_id}/approve")]
        public async Task<IActionResult> Approve([FromRoute(Name = "sell_id")] string sellId)
        {
            try
            {
                var sellData = await _sellService.GetByIdAsync(sellId);
                var session = User.GetSession();

                var lstStatus = await _transactionService.GetApproveStepAsync(sellData.Status, _productCode, _landCode_KSV);
                foreach (var item in lstStatus)
                {
                    switch (item.PhaseCode)
                    {
                        case "CHUYEN_NHUONG":
                            sellData.Status = item.ID;
                            sellData.UpdatedUser = session.UserName;
                            sellData.UpdatedDate = DateTime.Now;
                            sellData.ApprovedUser = session.UserName;
                            sellData.ApprovedDate = DateTime.Now;
                            await _sellService.ApproveAsync(sellData);
                            break;
                        case "NHAN_CHUYEN_NHUONG":
                            var buyData = await _buyService.GetLastBySellIdAsync(sellId);
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
                    Data = null
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorModel
                {
                    Code = "FAIL",
                    Message = e.Message,
                });
                throw;
            }
        }

        [HttpPost]
        [Route("{sell_id}/send")]
        public async Task<IActionResult> SendApprove([FromRoute(Name = "sell_id")] string sellId)
        {
            var sellData = await _sellService.GetByIdAsync(sellId);
            var session = User.GetSession();

            var lstStatus = await _transactionService.GetNextStepAsync(sellData.Status, _productCode, _landCode_GDV);
            foreach (var item in lstStatus)
            {
                switch (item.PhaseCode)
                {
                    case "CHUYEN_NHUONG":
                        sellData.Status = item.ID;
                        sellData.UpdatedUser = session.UserName;
                        sellData.UpdatedDate = DateTime.Now;
                        sellData.TellerUser = session.UserName;
                        sellData.TellerDate = DateTime.Now;
                        await _sellService.UpdateAsync(sellData);
                        break;
                    case "NHAN_CHUYEN_NHUONG":
                        var buyData = await _buyService.GetByIdAsync(sellId);
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

        [HttpPost]
        [Route("{sell_id}/reject")]
        public async Task<IActionResult> Reject([FromRoute(Name = "sell_id")] string sellId)
        {
            var sellData = await _sellService.GetByIdAsync(sellId);
            var session = User.GetSession();

            var lstStatus = await _transactionService.GetRejectStepAsync(sellData.Status, _productCode, _landCode_KSV);
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
                        var buyData = await _buyService.GetByIdAsync(sellId);
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

        [HttpGet]
        [Route("query")]
        public async Task<IActionResult> QueryAsync([FromQuery(Name = "account_balance_from")] decimal? balanceFrom,
                                                    [FromQuery(Name = "account_balance_to")] decimal? balanceTo,
                                                    [FromQuery(Name = "remain_day_from")] decimal? remainDateFrom,
                                                    [FromQuery(Name = "remain_day_to")] decimal? remainDateTo,
                                                    [FromQuery(Name = "status_code")] string status,
                                                       [FromQuery(Name = "account_class")] string accountClass,
                                                          [FromQuery(Name = "account_no")] string accountNo,
                                                             [FromQuery(Name = "cif")] string cif,
                                                             [FromQuery(Name = "transaction_date")] DateTime? transactionDate,
                                                    [FromQuery(Name = "page_num")] int pageNum,
                                                    [FromQuery(Name = "page_size")] int pageSize)
        {
            var session = User.GetSession();
            var usserBranchCode = session.BranchCode;
            var rs = await _sellService.QueryAsync(balanceFrom, balanceTo, remainDateFrom, remainDateTo, status, accountClass, accountNo, cif, transactionDate, usserBranchCode, pageNum, pageSize);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpGet]
        [Route("clean/query")]
        public async Task<IActionResult> CleanQuerySellAsync([FromQuery(Name = "page_num")] int pageNum,
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
        [Route("{sell_id}")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "sell_id")] string sellId)
        {
            var rs = await _sellService.GetByIdAsync(sellId);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!",
                Data = rs
            });
        }

        [HttpPost]
        [Route("{sell_id}/contract/upload")]
        public async Task<IActionResult> UploadContract([FromRoute(Name = "sell_id")] string sellId, IFormFile files)
        {
            IFormFile file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var sell = new SellContractModel();
                var fileName = $"{sellId}_HDSELL";
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    sell.SellFile = memoryStream.ToArray();
                }
                sell.FileName = fileName;
                sell.FileType = file.ContentType;
                var session = User.GetSession();
                sell.CreatedUser = session.UserName;
                sell.CreatedAt = DateTime.Now;
                sell.SellID = sellId;
                sell.Status = ContractConsts.CHO_DUYET;
                await _sellContractService.InsertAsync(sell);
            }

            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("{sell_id}/contract/reupload")]
        public async Task<IActionResult> ReuploadContract([FromRoute(Name = "sell_id")] string sellId, IFormFile files)
        {
            IFormFile file = Request.Form.Files[0];
            var rs = await _sellContractService.GetByIdAsync(sellId);


            var fileName = $"{sellId}_HDSELL";
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                rs.SellFile = memoryStream.ToArray();
            }
            rs.FileName = fileName;
            rs.FileType = file.ContentType;

            rs.Status = ContractConsts.CHO_DUYET;
            var session = User.GetSession();
            rs.UpdatedUser = session.UserName;
            rs.UpdatedAt = DateTime.Now;
            await _sellContractService.UpdateAsync(rs);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("{sell_id}/contract/approve")]
        public async Task<IActionResult> ApproveContract([FromRoute(Name = "sell_id")] string sellId)
        {
            var session = User.GetSession();
            var rs = await _sellContractService.GetByIdAsync(sellId);
            rs.ApprovedUser = session.UserName;
            rs.ApprovedAt = DateTime.Now;
            rs.Status = ContractConsts.DA_DUYET;
            await _sellContractService.UpdateAsync(rs);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });

        }

        [HttpGet]
        [Route("{sell_id}/contract/download")]
        public async Task<IActionResult> GetContract([FromRoute(Name = "sell_id")] string sellId)
        {
            var rs = await _sellContractService.GetByIdAsync(sellId);
            return File(rs.SellFile, rs.FileType, rs.FileName);
        }

        [HttpDelete]
        [Route("{sell_id}/contract")]
        public async Task<IActionResult> DeleteContract([FromRoute(Name = "sell_id")] string sellId)
        {
            await _sellContractService.DeleteAsync(sellId);
            return Ok(new ErrorModel
            {
                Code = "OK",
                Message = "Successful!"
            });
        }

        [HttpPost]
        [Route("{sell_id}/contract/preview")]
        public async Task<IActionResult> ContractSellPreviewAsync([FromRoute(Name = "sell_id")] string sellId)
        {
            var rs = await _sellService.GetByIdAsync(sellId);
            var rsInfoCus = await _soaService.RetrieveCustomerRefDataMgmt(rs.SellCif);
            var rsDetailAcc = await _soaService.RetrieveCurrentAccountCASA(rs.SellPaymentAccountNo);
            var branchInfo = await _branchService.GetByBranchID(rs.CreatedBranchCode);
            var output = rsInfoCus.retrieveCustomerRefDataMgmt_out;
            var cus = new CustomerModel
            {
                FullName = output.CustomerInfo?.Fullname,
                IdNo = output.CustomerInfo.IDInfo?.IDNum,
                IdDate = (DateTime.ParseExact(output.CustomerInfo.IDInfo?.IDIssuedDate,"yyyy-MM-dd hh:mm:ss.f", CultureInfo.CreateSpecificCulture("en-GB"))).ToString("dd/MM/yyyy"),
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
            var sellData = (rs.Data as object)?.MapProp<AccountTranFccModel>();
            OtherModel otherInfo = new OtherModel();
            switch (sellData.HinhThucLinhLai)
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
            otherInfo.SoDuVn = sellData.SoDu.ToString("#,##0");
            otherInfo.GiaTriGiaoDich = sellData.GiaTriGiaoDich.ToString("#,##0");
            otherInfo.NgayMo = sellData.NgayMo.ToString("dd/MM/yyyy");
            otherInfo.NgayTaiKy = sellData.NgayTaiKy.ToString("dd/MM/yyyy");
            otherInfo.LaiSuat = sellData.LaiSuat.ToString();
            otherInfo.SoTienCn = sellData.SoTienKhNhanCn.ToString("#,##0");
            otherInfo.SoTienThuePhi = sellData.SoTienThuPhi.ToString("#,##0");
            otherInfo.SoTienCnTxt = Converter.NumberToTextVN(sellData.SoTienKhNhanCn);
            otherInfo.BranchName = branchInfo.Name;
            otherInfo.NguoiDaiDien = sellData.NguoiDaiDien;
            if (rsDetailAcc.retrieveCurrentAccountCASA_out?.AccountInfo?.AccountOpenBrandCode != null)
            {
                var branchInfoAcc = await _branchService.GetByBranchID(rsDetailAcc.retrieveCurrentAccountCASA_out?.AccountInfo?.AccountOpenBrandCode);
                otherInfo.BranchNameAcc = branchInfoAcc.Name;
            }

            ReportModel rsData = new ReportModel
            {
                CustomerInfo = cus,
                DataInfo = sellData,
                SellInfo = rs,
                OtherInfo = otherInfo
            };
            string fileName = "";
            if (rs.SellPaymentMethod == "CK")
            {
                fileName = "HD_BAN_CHUYENKHOAN.doc";
            }
            else
            {
                fileName = "HD_BAN_TIENMAT.doc";
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
                "HD_CN_"+ output.CustomerInfo?.Fullname + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".doc");
        }
    }
}
