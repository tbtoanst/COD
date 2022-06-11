using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Transaction
{
    public class RequestCreateTransactionModel
    {
        /// <summary>
        /// Data Type: VARCHAR2(36 BYTE)
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(20 BYTE)
        /// </summary>
        [JsonProperty("account_num")]
        public string AccountNum { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("account_balance")]
        public decimal? AccountBalance { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("interest")]
        public decimal? Interest { get; set; }
        /// <summary>
        /// Data Type: DATE
        /// </summary>
        [JsonProperty("open_date")]
        public DateTime? OpenDate { get; set; }
        /// <summary>
        /// Data Type: DATE
        /// </summary>
        [JsonProperty("expired_date")]
        public DateTime? ExpiredDate { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("expired_day")]
        public decimal? ExpiredDay { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("buy_interest")]
        public decimal? BuyInterest { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("transaction_amount")]
        public decimal? TransactionAmount { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("buy_fee")]
        public decimal? BuyFee { get; set; }
        /// <summary>
        /// Data Type: NUMBER
        /// </summary>
        [JsonProperty("amount_fee")]
        public decimal? AmountFee { get; set; }
        /// <summary>
        /// Data Type: DATE
        /// </summary>
        [JsonProperty("transaction_date")]
        public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(7 BYTE)
        /// </summary>
        [JsonProperty("buy_cif")]
        public string BuyCif { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(3 BYTE)
        /// </summary>
        [JsonProperty("buy_branch_code")]
        public string BuyBranchCode { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(20 BYTE)
        /// </summary>
        [JsonProperty("created_user")]
        public string CreatedUser { get; set; }
        /// <summary>
        /// Data Type: DATE
        /// </summary>
        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(1 BYTE)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(36 BYTE)
        /// </summary>
        [JsonProperty("buy_id")]
        public string buy_id { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(3 BYTE)
        /// </summary>
        [JsonProperty("ccy")]
        public string Ccy { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(100 BYTE)
        /// </summary>
        [JsonProperty("cus_full_name")]
        public string CusFullName { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(3 BYTE)
        /// </summary>
        [JsonProperty("priority_branch_code")]
        public string PriorityBranchCode { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(20 BYTE)
        /// </summary>
        [JsonProperty("priority_seller")]
        public string PrioritySeller { get; set; }
        /// <summary>
        /// Data Type: DATE
        /// </summary>
        [JsonProperty("matched_date")]
        public DateTime? MatchedbnDate { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(100 BYTE)
        /// </summary>
        [JsonProperty("advance_status")]
        public string AdvanceStatus { get; set; }
        /// <summary>
        /// Data Type: VARCHAR2(100 BYTE)
        /// </summary>
        [JsonProperty("contract_no")]
        public string ContractNo { get; set; }
    }

    public class RequestGetListTransactionModel
    {
        /// <summary>
        /// Data Type: VARCHAR2(3 BYTE)
        /// </summary>
        [JsonProperty("branch_code")]
        [FromQuery(Name = "branch_code")]
        public string BranchCode { get; set; }

        [JsonProperty("from_date")]
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("to_date")]
        [FromQuery(Name = "to_date")]
        public DateTime? toDate { get; set; }

        [JsonProperty("contract_no")]
        [FromQuery(Name = "contract_no")]
        public string ContractNo { get; set; }

        [JsonProperty("status_code")]
        [FromQuery(Name = "status_code")]
        public string Status { get; set; }

        [JsonProperty("acc_num")]
        [FromQuery(Name = "acc_num")]
        public string AccountNum { get; set; }

        [JsonProperty("page_num")]
        [FromQuery(Name = "page_num")]
        public int PageNum { get; set; }

        [JsonProperty("page_size")]
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }
    }

    public class RequestTransactionLockModel
    {
        /// <summary>
        /// input = DONG: đóng ngày; MO: mở ngày
        /// </summary>
        [JsonProperty("status")]
        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }
}
