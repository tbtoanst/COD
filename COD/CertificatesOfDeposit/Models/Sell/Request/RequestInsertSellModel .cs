using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Sell.Request
{
    public class RequestInsertSellModel
    {
        [JsonProperty("sell_cif")]
        public string SellCif { get; set; }

        [JsonProperty("sell_payment_method")]
        public string SellPaymentMethod { get; set; }

        [JsonProperty("sell_payment_account_no")]
        public string SellPaymentAccountNo { get; set; }

        [JsonProperty("kpi_indirect")]
        public string KpiIndirect { get; set; }

        [JsonProperty("kpi_direct")]
        public string KpiDirect { get; set; }

        [JsonProperty("sell_payment_ccy")]
        public string SellPaymentCCY { get; set; }

        [JsonProperty("account_num_list")]
        public List<string> AccountNumList { get; set; }

        [JsonProperty("branch_account_no")]
        public string BranchAccountNo { get; set; }
    }
}
