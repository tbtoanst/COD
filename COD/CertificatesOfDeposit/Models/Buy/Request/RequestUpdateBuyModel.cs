using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell.Request
{
    public class RequestUpdateBuyModel
    {

        [JsonProperty("seri_no")]
        public string SeriNo { get; set; }

        [JsonProperty("buy_payout_acc_num")]
        public string BuyPayoutAccNum { get; set; }

    }
}
