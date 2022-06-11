using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Buy
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("sell_id")]
        public string SellId { get; set; }
        [Column("booking_id")]
        public string BookingId { get; set; }
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }
        [Column("waiting_time")]
        public decimal WaitingTime { get; set; }
        [Column("buy_cif")]
        public string BuyCif { get; set; }
        [Column("buy_fullname")]
        public string BuyFullname { get; set; }
        [Column("buy_phone")]
        public string BuyPhone { get; set; }
        [Column("buy_address")]
        public string BuyAddress { get; set; }
        [Column("buy_id_num")]
        public string BuyIdNum { get; set; }

        [Column("buy_payment_method")]
        public string BuyPaymentMethod { get; set; }
        [Column("buy_payment_account_no")]
        public string BuyPaymentAccountNo { get; set; }
        [Column("buy_payment_ccy")]
        public string BuyPaymentCCY { get; set; }
        [Column("buy_payout_method")]
        public string BuyPayoutMethod { get; set; }
        [Column("buy_payout_acc_num")]
        public string BuyPayoutAccNum { get; set; }
        [Column("buy_account_balance")]
        public decimal BuyAccountBalance { get; set; }
        [Column("buy_branch_code")]
        public string BuyBranchCode { get; set; }
        [Column("seri_no")]
        public string SeriNo { get; set; }
        [Column("erp_extrastring")]
        public string ErpExtrastring { get; set; }
        [Column("erp_seriowner")]
        public string ErpSeriowner { get; set; }
        [Column("kpi_indirect")]
        public string KpiIndirect { get; set; }
        [Column("kpi_direct")]
        public string KpiDirect { get; set; }
        [Column("xref_id")]
        public string XrefId { get; set; }
        [Column("xref_fee_dttg")]
        public string XrefFeeDttg { get; set; }
        [Column("created_user")]
        public string CreatedUser { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("created_branch_code")]
        public string CreatedBranchCode { get; set; }
        [Column("teller_user")]
        public string TellerUser { get; set; }
        [Column("teller_date")]
        public DateTime? TellerDate { get; set; }
        [Column("approved_user")]
        public string ApprovedUser { get; set; }
        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }
        [Column("deleted_user")]
        public string DeletedUser { get; set; }
        [Column("deleted_date")]
        public DateTime? DeletedDate { get; set; }
        [Column("status")]
        public string Status { get; set; }

        [Column("buy_contract")]
        public BuyContract BuyContract { get; set; }

        [Column("sell")]
        public Sell Sell { get; set; }

        [Column("data")]
        public string Data { get; set; }
        [Column("updated_user")]
        public string UpdatedUser { get; set; }
        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }
        [Column("created_user_full_name")]
        public string CreatedUserFullName { get; set; }
        [Column("updated_user_full_name")]
        public string UpdatedUserFullName { get; set; }
        [Column("teller_user_full_name")]
        public string TellerUserFullName { get; set; }
        [Column("approved_user_full_name")]
        public string ApprovedUserFullName { get; set; }
        [Column("deleted_user_full_name")]
        public string DeletedUserFullName { get; set; }

        [Column("branch_account_no")]
        public string BranchAccountNo { get; set; }
        [Column("buy_payment_branch_code")]
        public string BuyPaymentBranchCode { get; set; }

        [Column("buy_payout_branch_code")]
        public string BuyPayoutBranchCode { get; set; }

        [Column("fcc_ref")]
        public string FccRef { get; set; }

        [Column("buy_payment_ballance")]
        public decimal BuyPaymentBallance { get; set; }
        [Column("buy_payment_fee")]
        public decimal BuyPaymentFee { get; set; }
    }
}
