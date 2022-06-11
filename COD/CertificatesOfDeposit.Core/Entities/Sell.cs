using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Sell
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("booking_id")]
        public string BookingId { get; set; }

        [Column("sell_cif")]
        public string SellCif { get; set; }

        [Column("account_num")]
        public string AccountNum { get; set; }

        [Column("account_balance")]
        public decimal AccountBalance { get; set; }

        [Column("account_class")]
        public string AccountClass { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Column("remain_day")]
        public decimal RemainDay { get; set; }

        [Column("priority_branch_code")]
        public string PriorityBranchCode { get; set; }

        [Column("priority_seller")]
        public string PrioritySeller { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("created_user")]
        public string CreatedUser { get; set; }

        [Column("created_branch_code")]
        public string CreatedBranchCode { get; set; }

        [Column("approved_user")]
        public string ApprovedUser { get; set; }

        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [Column("teller_user")]
        public string TellerUser { get; set; }

        [Column("teller_date")]
        public DateTime? TellerDate { get; set; }

        [Column("deleted_user")]
        public string DeletedUser { get; set; }

        [Column("deleted_date")]
        public DateTime? DeletedDate { get; set; }

        [Column("kpi_indirect")]
        public string KpiIndirect { get; set; }

        [Column("kpi_direct")]
        public string KpiDirect { get; set; }

        [Column("sell_fullname")]
        public string SellFullname { get; set; }

        [Column("sell_phone")]
        public string SellPhone { get; set; }

        [Column("sell_address")]
        public string SellAddress { get; set; }

        [Column("sell_payment_method")]
        public string SellPaymentMethod { get; set; }

        [Column("sell_id_num")]
        public string SellIdNum { get; set; }

        [Column("sell_payment_account_no")]
        public string SellPaymentAccountNo { get; set; }

        [Column("sell_payment_ccy")]
        public string SellPaymentCCY { get; set; }

        [Column("sell_branch_code")]
        public string SellBranchCode { get; set; }

        [Column("sell_contract")]
        public SellContract SellContract { get; set; }

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
        [Column("sell_payment_branch_code")]
        public string SellPaymentBranchCode { get; set; }

        [Column("xref_id")]
        public string XrefId { get; set; }

        [Column("fcc_ref")]
        public string FccRef { get; set; }

        [Column("sell_payment_ballance")]
        public decimal SellPaymentBallance { get; set; }
        [Column("sell_payment_fee")]
        public decimal SellPaymentFee { get; set; }
    }
}
