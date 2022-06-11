using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class RetrieveCoreBankAccount
    {
        public class RetrieveCoreBankAccountIn
        {
            [JsonProperty("retrieveCoreBankAccount_in")]
            public RetrieveCoreBankAccount_In retrieveCoreBankAccount_in { get; set; }
        }

        public class RetrieveCoreBankAccount_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("coreBankAccount")]
            public CoreBankAccountModel CoreBankAccount { get; set; }
        }

        public class CoreBankAccountModel
        {
            [JsonProperty("userAccount")]
            public string UserAccount { get; set; }
        }

        public class RetrieveCoreBankAccountOut
        {
            [JsonProperty("retrieveCoreBankAccount_out")]
            public RetrieveCoreBankAccount_Out RetrieveCoreBankAccount_out { get; set; }
        }

        public class RetrieveCoreBankAccount_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            public CoreBankAccount CoreBankAccount { get; set; }            
        }
    }
}
