using CertificatesOfDeposit.Models.Sell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.AccountTranFcc
{
    public class AccountDetailCASAModel
    {
        [JsonProperty("cust_ac_no")]
        public string custAcNo { get; set; }
        [JsonProperty("ccy")]
        public string Ccy { get; set; }
        [JsonProperty("ac_desc")]
        public string acDesc { get; set; }
        [JsonProperty("account_class")]
        public string accountClass { get; set; }
        [JsonProperty("cust_no")]
        public string custNo { get; set; }
        [JsonProperty("act_type")]
        public string actType { get; set; }
        [JsonProperty("current_balance")]
        public string currentBalance { get; set; }
        [JsonProperty("trang_thai")]
        public string trangThai { get; set; }
    }
}
