using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell
{
    public class SellModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("booking_id")]
        public string BookingId { get; set; }

        [JsonProperty("sell_cif")]
        public string SellCif { get; set; }

        [JsonProperty("account_num")]
        public string AccountNum { get; set; }

        [JsonProperty("account_balance")]
        public decimal AccountBalance { get; set; }

        [JsonProperty("account_class")]
        public string AccountClass { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty("remain_day")]
        public int RemainDay { get; set; }

        [JsonProperty("priority_branch_code")]
        public string PriorityBranchCode { get; set; }

        [JsonProperty("priority_seller")]
        public string PrioritySeller { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("created_user")]
        public string CreatedUser { get; set; }

        [JsonProperty("created_branch_code")]
        public string CreatedBranchCode { get; set; }

        [JsonProperty("approved_user")]
        public string ApprovedUser { get; set; }

        [JsonProperty("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [JsonProperty("teller_user")]
        public string TellerUser { get; set; }

        [JsonProperty("teller_date")]
        public DateTime? TellerDate { get; set; }

        [JsonProperty("deleted_user")]
        public string DeletedUser { get; set; }

        [JsonProperty("deleted_date")]
        public DateTime? DeletedDate { get; set; }

        [JsonProperty("kpi_indirect")]
        public string KpiIndirect { get; set; }

        [JsonProperty("kpi_direct")]
        public string KpiDirect { get; set; }

        [JsonProperty("sell_fullname")]
        public string SellFullname { get; set; }

        [JsonProperty("sell_phone")]
        public string SellPhone { get; set; }

        [JsonProperty("sell_address")]
        public string SellAddress { get; set; }

        [JsonProperty("sell_payment_method")]
        public string SellPaymentMethod { get; set; }

        [JsonProperty("sell_payment_ccy")]
        public string SellPaymentCCY { get; set; }

        [JsonProperty("sell_id_num")]
        public string SellIdNum { get; set; }

        [JsonProperty("sell_payment_account_no")]
        public string SellPaymentAccountNo { get; set; }

        [JsonProperty("sell_branch_code")]
        public string SellBranchCode { get; set; }

        [JsonProperty("sell_contract")]
        public SellContractModel SellContract { get; set; }

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
        [JsonProperty("sell_payment_branch_code")]
        public string SellPaymentBranchCode { get; set; }

        [JsonProperty("xref_id")]
        public string XrefId { get; set; }

        [JsonProperty("fcc_ref")]
        public string FccRef { get; set; }
        [JsonProperty("sell_payment_ballance")]
        public decimal SellPaymentBallance { get; set; }
        [JsonProperty("sell_payment_fee")]
        public decimal SellPaymentFee { get; set; }

    }
}
