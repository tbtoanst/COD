using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Sell.Request
{
    public class RequestInsertBuyModel
    {
        [JsonProperty("sell_ids")]
        public List<string> SellIds { get; set; }

        [JsonProperty("kpi_indirect")]
        public string KpiIndirect { get; set; }

        [JsonProperty("kpi_direct")]
        public string KpiDirect { get; set; }
        
        [JsonProperty("buy_payment_method")]
        public string BuyPaymentMethod { get; set; }

        [JsonProperty("buy_payment_ccy")]
        public string BuyPaymentCCY { get; set; }
        [JsonProperty("buy_payout_method")]
        public string BuyPayoutMethod { get; set; }
        [JsonProperty("buy_payout_acc_num")]
        public string BuyPayoutAccNum { get; set; }

        [JsonProperty("buy_payment_account_no")]
        public string BuyPaymentAccountNo { get; set; }

        [JsonProperty("buy_cif")]
        public string BuyCif { get; set; }


        [JsonProperty("branch_account_no")]
        public string BranchAccountNo { get; set; }



    }
}
