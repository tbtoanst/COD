using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class SelectBookAccModel
    {
        public class SelectBookAccIn
        {
            [JsonProperty("selectBookAccFromCIF_in")]
            public Selectbookacc_In selectBookAccFromCIF_in { get; set; }
        }

        public class SelectBookAccOut
        {
            [JsonProperty("selectBookAcc_out")]
            public Selectbookacc_Out selectBookAcc_out { get; set; }
        }

        public class Selectbookacc_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("CIFInfo")]
            public CifInfoModel CIFInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel AccountInfo { get; set; }
        }

        public class Selectbookacc_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel[] AccountInfo { get; set; }
        }
    }
}
