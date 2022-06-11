using CertificatesOfDeposit.API.Models.Account;
using CertificatesOfDeposit.Core.Entities;
using CertificatesOfDeposit.Helpers;
using CertificatesOfDeposit.Infrastructure.Repository;
using CertificatesOfDeposit.Models;
using CertificatesOfDeposit.Models.Account;
using CertificatesOfDeposit.Models.SOA;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static CertificatesOfDeposit.Models.SOA.CheckAccountCoreBankModel;
using static CertificatesOfDeposit.Models.SOA.RetrieveCustomerRefDataMgmtModel;
using static CertificatesOfDeposit.Models.SOA.SelectCustomerRefDataMgmtCIFNumModel;
using static CertificatesOfDeposit.Models.SOA.SelectDirectSellerTypeOfBranchModel;
using static CertificatesOfDeposit.Models.SOA.SelectBookAccModel;
using static CertificatesOfDeposit.Models.SOA.RetreiveSerialNumberModel;
using static CertificatesOfDeposit.Models.SOA.RetrieveCurrentAccountCASA;
using static CertificatesOfDeposit.Models.SOA.SelectCurrentAccountFromCIFModel;
using static CertificatesOfDeposit.Models.SOA.RetrieveCoreBankAccount;

namespace CertificatesOfDeposit.Services.SOA
{
    public interface ISOAService
    {
        /// <summary>
        /// Check user and password exists on Core Banking
        /// </summary>
        /// <param name="username">Required</param>
        /// <param name="password">Required</param>
        /// <returns></returns>
        Task<CheckAccountCoreBank_out> CheckUserCoreBankingAsync(string username, string password);

        /// <summary>
        /// Get customer info from Core Banking throw SOA
        /// </summary>
        /// <param name="CIF">CIF 7 characters</param>
        /// <returns></returns>
        Task<RetrieveCustomerRefDataMgmt_out> RetrieveCustomerRefDataMgmt(string CIF);

        /// <summary>
        /// Get customer info from Core Banking throw SOA
        /// </summary>
        /// <param name="CIF"></param>
        /// <param name="IDNumber"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<SelectCustomerrefDataMgmtCifNumOut> SelectCustomerRefDataMgmtCIFNum(string CIF, string IDNumber, string phoneNumber);

        /// <summary>
        /// Get list customer in a branch
        /// </summary>
        /// <param name="emmployeeType">I(Indirect) or D (Direct)</param>
        /// <param name="branchID">3 characters</param>
        /// <returns></returns>
        Task<SelectDirectSellerTypeOfBranchOut> SelectDirectSellerTypeOfBranch(string emmployeeType, string branchID);
        
        /// <summary>
        /// Get payment account
        /// </summary>
        /// <param name="CIF"></param>
        /// <param name="branchCode"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        Task<SelectBookAccOut> selectBookAccFromCIF(string CIF);

        /// <summary>
        /// Get info Book saving
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="code"></param>
        /// <param name="branchCode"></param>
        /// <param name="makerAcc"></param>
        /// <returns></returns>
        Task<RetreiveSerialNumberOut> RetreiveSerialNumber(string prefix, string code, string branchCode, string makerAcc);
        
        /// <summary>
        /// Get account info form CASA
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        Task<RetrieveCurrentAccountCASAOut> RetrieveCurrentAccountCASA(string accountNumber);
        
        /// <summary>
        /// Get list current account from Branch code
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        Task<SelectCurrentAccountFromCIFOut> SelectCurrentAccountFromCIF(string branchCode);
        
        /// <summary>
        /// Find username exsts in Core banking
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<RetrieveCoreBankAccountOut> RetrieveCoreBankAccount(string userName);
    }

    public class SOAService: ISOAService
    {
        private readonly IConfiguration _config;
        private static string _clientCode = AppConsts._productCode;
        private static string _clientCodeFCC = "ERPDB";
        private static string _branchCode = "000";

        public SOAService(IConfiguration configuration)
        {
            this._config = configuration;
        }

        private TransactionInfoModel BuidTransactionInfo(string clientCode = null)
        {
            return new TransactionInfoModel
            {
                ClientCode = string.IsNullOrEmpty(clientCode) ? _clientCode : clientCode,
                CRefNum = Guid.NewGuid().ToString("N"),
                BranchInfo = new BranchInfoModel
                {
                    BranchCode = _branchCode
                }
            };
        }

        public async Task<CheckAccountCoreBank_out> CheckUserCoreBankingAsync(string username, string password)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/systemadmin/v1.0/rest/checkAccountCoreBank");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                CheckAccountCoreBank_in input = new CheckAccountCoreBank_in
                {
                    checkAccountCoreBank_in = new CheckAccountCoreBank_in.CheckAccountCorebank_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        AuthenAccountInfo = new CheckAccountCoreBank_in.AuthenAccountInfo
                        {
                            userName = username.ToUpper(),
                            passWord = password
                        }
                    }
                };
                request.AddJsonBody(input);
                var response = await restClient.ExecuteAsync(request);
                var output = JsonConvert.DeserializeObject<CheckAccountCoreBank_out>(response.Content);
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<RetrieveCustomerRefDataMgmt_out> RetrieveCustomerRefDataMgmt(string CIF)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/customerrefdatamgmt/v1.0/rest/retrieveCustomerRefDataMgmt");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                RetrieveCustomerRefDataMgmt_in input = new RetrieveCustomerRefDataMgmt_in
                {
                    retrieveCustomerRefDataMgmt_in = new RetrievecustomerrefdataMgmt_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        CIFInfo = new CifInfoModel
                        {
                            CIFNum = CIF,
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var output = JsonConvert.DeserializeObject<RetrieveCustomerRefDataMgmt_out>(response.Content);
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectCustomerrefDataMgmtCifNumOut> SelectCustomerRefDataMgmtCIFNum(string CIF, string IDNumber, string phoneNumber)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/customerrefdatamgmt/v1.0/rest/selectCustomerRefDataMgmtCIFNum");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                SelectCustomerRefDataMgmtCIFNumIn input = new SelectCustomerRefDataMgmtCIFNumIn
                {
                    SelectCustomerRefDataMgmtCIFNum_in = new SelectCustomerRefDataMgmtCIFNum_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        CIFInfo = new CifInfoModel
                        {
                            CIFNum = CIF,
                        },
                        CustomerInfo = new CustomerInfoModel
                        {
                            IDInfo = new IdInfoModel
                            {
                                IDNum = IDNumber
                            },
                            Address = new AddressModel
                            {
                                PhoneNum = phoneNumber
                            }
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var output = JsonConvert.DeserializeObject<SelectCustomerrefDataMgmtCifNumOut>(response.Content);
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectDirectSellerTypeOfBranchOut> SelectDirectSellerTypeOfBranch(string emmployeeType, string branchID)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/empdatamgmt/v1.0/rest/selectDirectSellerTypeOfBranch");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                SelectDirectSellerTypeOfBranchIn input = new SelectDirectSellerTypeOfBranchIn
                {
                    SelectCustomerRefDataMgmtCIFNum_in = new SelectDirectSellerTypeOfBranch_in
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        HrEmpDataInfo = new HrEmpDataInfoModel
                        {
                            HrLoaiNhanVien = emmployeeType,
                            HrMaDonViGoc = branchID,
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var output = JsonConvert.DeserializeObject<SelectDirectSellerTypeOfBranchOut>(response.Content);
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectBookAccOut> selectBookAccFromCIF(string CIF)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/currentaccount/v1.0/rest/selectBookAccFromCIF");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                SelectBookAccIn input = new SelectBookAccIn
                {
                    selectBookAccFromCIF_in = new Selectbookacc_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        CIFInfo = new CifInfoModel
                        {
                            CIFNum = CIF,
                            BranchCode = ""
                        },
                        AccountInfo = new AccountInfoModel
                        {
                            AccountCurrency = "VND"
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var rsAcc = JsonConvert.DeserializeObject<SelectBookAccOut>(response.Content);

                var cifInfo = RetrieveCustomerRefDataMgmt(CIF).Result.retrieveCustomerRefDataMgmt_out;
                string accountClassCode = "CAI";
                if (cifInfo.CustomerInfo.NationlityCode == "VN")
                {
                    accountClassCode = "CAI";
                }
                else
                {
                    //SegmentType: I_12 nguoi nuoc ngoai
                    if (cifInfo.CustomerInfo.SegmentType == "I_12")
                    {
                        accountClassCode = "CDI";
                    }
                }
                return new SelectBookAccOut
                {
                    selectBookAcc_out = new Selectbookacc_Out
                    {
                        TransactionInfo = rsAcc.selectBookAcc_out.TransactionInfo,
                        AccountInfo = rsAcc.selectBookAcc_out.AccountInfo.Where(b => b.AccountClassCode.StartsWith(accountClassCode)).ToArray()
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RetreiveSerialNumberOut> RetreiveSerialNumber(string prefix, string code, string branchCode, string makerAcc)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/fccservices/v1.0/rest/retreiveSerialNumber");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                RetreiveSerialNumberIn input = new RetreiveSerialNumberIn
                {
                    RetreiveSerialNumber_in = new RetreiveSerialNumber_In
                    {
                        TransactionInfo = BuidTransactionInfo(_clientCodeFCC),
                        ErpInfo = new ErpInfo
                        {
                            Prefix = prefix,
                            Code = code,
                            BranchCode = new ErpBranchCode
                            {
                                BranchCode = branchCode,
                            }
                        },
                        CoreBankAccount = new CoreBankAccount
                        {
                            MakerAccount = makerAcc
                        }

                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var output = JsonConvert.DeserializeObject<RetreiveSerialNumberOut>(response.Content);
                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RetrieveCurrentAccountCASAOut> RetrieveCurrentAccountCASA(string AccountNumber)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/currentaccount/v1.0/rest/retrieveCurrentAccountCASA");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                RetrieveCurrentAccountCASAIn input = new RetrieveCurrentAccountCASAIn
                {
                    retrieveCurrentAccountCASA_in = new RetrieveCurrentAccountCASA_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        AccountInfo = new AccountInfoModel
                        {
                            AccountNum = AccountNumber
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var rsAcc = JsonConvert.DeserializeObject<RetrieveCurrentAccountCASAOut>(response.Content);
                return rsAcc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectCurrentAccountFromCIFOut> SelectCurrentAccountFromCIF(string branchCode)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/currentaccount/v1.0/rest/selectCurrentAccountFromCIF");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                SelectCurrentAccountFromCIFIn input = new SelectCurrentAccountFromCIFIn
                {
                    SelectCurrentAccountFromCIF_in = new selectCurrentAccountFromCIF_In
                    {
                        TransactionInfo = BuidTransactionInfo(),
                        CIFInfo = new CifInfoModel
                        {
                            CIFNum = null,
                            BranchCode = branchCode
                        },
                        AccountInfo = new AccountInfoModel
                        {
                            AccountCurrency = "VND"
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var rsAcc = JsonConvert.DeserializeObject<SelectCurrentAccountFromCIFOut>(response.Content);
                return rsAcc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RetrieveCoreBankAccountOut> RetrieveCoreBankAccount(string userName)
        {
            try
            {
                var url = _config["SOA:urlSOA"];
                var codeAuth = _config["SOA:auth"];

                Uri baseUrl = new Uri($"{url}/systemadmin/v1.0/rest/retrieveCoreBankAccount");
                RestClient restClient = new RestClient(baseUrl);
                RestRequest request = new RestRequest("", Method.Post);
                request.AddHeader("Authorization", codeAuth);
                request.AddHeader("Content-Type", "application/json");
                RetrieveCoreBankAccountIn input = new RetrieveCoreBankAccountIn
                {
                    retrieveCoreBankAccount_in = new RetrieveCoreBankAccount_In
                    {
                        TransactionInfo = BuidTransactionInfo("LOCALPAYMENT"),
                        CoreBankAccount = new CoreBankAccountModel
                        {
                            UserAccount = userName,
                        }
                    }
                };
                request.AddStringBody(input.SerializeObject(), DataFormat.Json);
                var response = await restClient.ExecuteAsync(request);
                var rsAcc = JsonConvert.DeserializeObject<RetrieveCoreBankAccountOut>(response.Content);
                return rsAcc;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
