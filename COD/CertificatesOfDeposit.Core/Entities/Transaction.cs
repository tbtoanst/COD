using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Transaction
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("account_num")]
        public string AccountNum { get; set; }

        [Column("account_balance")]
        public decimal? AccountBalance { get; set; }

        [Column("interest")]
        public decimal? Interest { get; set; }

        [Column("open_date")]
        public DateTime? OpenDate { get; set; }

        [Column("expired_date")]
        public DateTime? ExpiredDate { get; set; }

        [Column("expired_day")]
        public decimal? ExpiredDay { get; set; }

        [Column("buy_interest")]
        public decimal? BuyInterest { get; set; }

        [Column("transaction_amount")]
        public decimal? TransactionAmount { get; set; }

        [Column("buy_fee")]
        public decimal? BuyFee { get; set; }

        [Column("amount_fee")]
        public decimal? AmountFee { get; set; }

        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        [Column("buy_cif")]
        public string BuyCif { get; set; }

        [Column("buy_branch_code")]
        public string BuyBranchCode { get; set; }

        [Column("created_user")]
        public string CreatedUser { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("buy_id")]
        public string BuyId { get; set; }

        [Column("sell_id")]
        public string SellId { get; set; }

        [Column("ccy")]
        public string Ccy { get; set; }

        [Column("cus_full_name")]
        public string CusFullName { get; set; }

        [Column("priority_branch_code")]
        public string PriorityBranchCode { get; set; }

        [Column("priority_seller")]
        public string PrioritySeller { get; set; }

        [Column("matched_date")]
        public DateTime? MatchedDate { get; set; }

        [Column("advance_status")]
        public string AdvanceStatus { get; set; }

        [Column("contract_no")]
        public string ContractNo { get; set; }

        [Column("buy")]
        public Buy? Buy { get; set; }

        [Column("sell")]
        public Sell? Sell { get; set; }
    }
}
