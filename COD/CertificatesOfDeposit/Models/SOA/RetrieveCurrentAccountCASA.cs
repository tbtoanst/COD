using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class RetrieveCurrentAccountCASA
    {
        public class RetrieveCurrentAccountCASAIn
        {
            [JsonProperty("retrieveCurrentAccountCASA_in")]
            public RetrieveCurrentAccountCASA_In retrieveCurrentAccountCASA_in { get; set; }
        }

        public class RetrieveCurrentAccountCASAOut
        {
            [JsonProperty("retrieveCurrentAccountCASA_out")]
            public RetrieveCurrentAccountCASA_Out retrieveCurrentAccountCASA_out { get; set; }
        }

        public class RetrieveCurrentAccountCASA_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel AccountInfo { get; set; }
        }

        public class RetrieveCurrentAccountCASA_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("accountInfo")]
            public AccountInfoModel AccountInfo { get; set; }
        }
    }
}
