using CertificatesOfDeposit.Models.Sell;
using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Buy
{
    public class BuyModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("sell_id")]
        public string SellId { get; set; }
        [JsonProperty("booking_id")]
        public string BookingId { get; set; }
        [JsonProperty("transaction_date")]
        public DateTime TransactionDate { get; set; }
        [JsonProperty("waiting_time")]
        public decimal WaitingTime { get; set; }
        [JsonProperty("buy_cif")]
        public string BuyCif { get; set; }
        [JsonProperty("buy_fullname")]
        public string BuyFullname { get; set; }
        [JsonProperty("buy_phone")]
        public string BuyPhone { get; set; }
        [JsonProperty("buy_address")]
        public string BuyAddress { get; set; }
        [JsonProperty("buy_id_num")]
        public string BuyIdNum { get; set; }

        [JsonProperty("buy_payment_method")]
        public string BuyPaymentMethod { get; set; }

        [JsonProperty("buy_payment_account_no")]
        public string BuyPaymentAccountNo { get; set; }
        [JsonProperty("buy_payment_ccy")]
        public string BuyPaymentCCY { get; set; }
        [JsonProperty("buy_payout_method")]
        public string BuyPayoutMethod { get; set; }
        [JsonProperty("buy_payout_acc_num")]
        public string BuyPayoutAccNum { get; set; }
        [JsonProperty("buy_account_balance")]
        public decimal BuyAccountBalance { get; set; }
        [JsonProperty("buy_branch_code")]
        public string BuyBranchCode { get; set; }
        [JsonProperty("seri_no")]
        public string SeriNo { get; set; }
        [JsonProperty("erp_extrastring")]
        public string ErpExtrastring { get; set; }
        [JsonProperty("erp_seriowner")]
        public string ErpSeriowner { get; set; }
        [JsonProperty("kpi_indirect")]
        public string KpiIndirect { get; set; }
        [JsonProperty("kpi_direct")]
        public string KpiDirect { get; set; }
        [JsonProperty("xref_id")]
        public string XrefId { get; set; }
        [JsonProperty("xref_fee_dttg")]
        public string XrefFeeDttg { get; set; }
        [JsonProperty("created_user")]
        public string CreatedUser { get; set; }
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("created_branch_code")]
        public string CreatedBranchCode { get; set; }
        [JsonProperty("teller_user")]
        public string TellerUser { get; set; }
        [JsonProperty("teller_date")]
        public DateTime? TellerDate { get; set; }
        [JsonProperty("approved_user")]
        public string ApprovedUser { get; set; }
        [JsonProperty("approved_date")]
        public DateTime? ApprovedDate { get; set; }
        [JsonProperty("deleted_user")]
        public string DeletedUser { get; set; }
        [JsonProperty("deleted_date")]
        public DateTime? DeletedDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("buy_contract")]
        public BuyContractModel BuyContract { get; set; }

        [JsonProperty("sell")]
        public SellModel Sell { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
        [JsonProperty("updated_user")]
        public string UpdatedUser { get; set; }
        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }
        [JsonProperty("created_user_full_name")]
        public string CreatedUserFullName { get; set; }
        [JsonProperty("updated_user_full_name")]
        public string UpdatedUserFullName { get; set; }
        [JsonProperty("teller_user_full_name")]
        public string TellerUserFullName { get; set; }
        [JsonProperty("approved_user_full_name")]
        public string ApprovedUserFullName { get; set; }
        [JsonProperty("deleted_user_full_name")]
        public string DeletedUserFullName { get; set; }

        [JsonProperty("branch_account_no")]
        public string BranchAccountNo { get; set; }
        [JsonProperty("buy_payment_branch_code")]
        public string BuyPaymentBranchCode { get; set; }

        [JsonProperty("buy_payout_branch_code")]
        public string BuyPayoutBranchCode { get; set; }

        [JsonProperty("fcc_ref")]
        public string FccRef { get; set; }
        [JsonProperty("buy_payment_ballance")]
        public decimal BuyPaymentBallance { get; set; }
        [JsonProperty("buy_payment_fee")]
        public decimal BuyPaymentFee { get; set; }
    }
}
