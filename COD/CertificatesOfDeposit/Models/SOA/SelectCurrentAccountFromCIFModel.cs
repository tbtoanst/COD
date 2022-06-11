using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class SelectCurrentAccountFromCIFModel
    {

        public class SelectCurrentAccountFromCIFIn
        {
            [JsonProperty("selectCurrentAccountFromCIF_in")]
            public selectCurrentAccountFromCIF_In SelectCurrentAccountFromCIF_in { get; set; }
        }

        public class selectCurrentAccountFromCIF_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("CIFInfo")]
            public CifInfoModel CIFInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel AccountInfo { get; set; }
        }


        public class SelectCurrentAccountFromCIFOut
        {
            [JsonProperty("selectCurrentAccountFromCIF_out")]
            public SelectCurrentAccountFromCIF_Out SelectCurrentAccountFromCIF_out { get; set; }
        }

        public class SelectCurrentAccountFromCIF_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel[] AccountInfo { get; set; }
        }
    }
}
