using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class RetreiveSerialNumberModel
    {

        public class RetreiveSerialNumberIn
        {
            [JsonProperty("retreiveSerialNumber_in")]
            public RetreiveSerialNumber_In RetreiveSerialNumber_in { get; set; }
        }

        public class RetreiveSerialNumber_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("erpInfo")]
            public ErpInfo ErpInfo { get; set; }
            [JsonProperty("coreBankAccount")]
            public CoreBankAccount CoreBankAccount { get; set; }
        }

        public class RetreiveSerialNumberOut
        {
            [JsonProperty("retreiveSerialNumber_out")]
            public RetreiveSerialNumber_Out RetreiveSerialNumber_out { get; set; }
        }

        public class RetreiveSerialNumber_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("erpInfo")]
            public ErpInfo ErpInfo { get; set; }
        }
    }
}
