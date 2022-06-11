using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.SOA
{
    public class CheckAccountCoreBankModel
    {
        public class CheckAccountCoreBank_in
        {
            [JsonProperty("checkAccountCoreBank_in")]
            public CheckAccountCorebank_In checkAccountCoreBank_in { get; set; }

            public class CheckAccountCorebank_In
            {
                [JsonProperty("transactionInfo")]
                public TransactionInfoModel TransactionInfo { get; set; }
                [JsonProperty("authenAccountInfo")]
                public AuthenAccountInfo AuthenAccountInfo { get; set; }
            }

            public class AuthenAccountInfo
            {
                [JsonProperty("userName")]
                public string userName { get; set; }
                [JsonProperty("passWord")]
                public string passWord { get; set; }
            }
        }

        public class CheckAccountCoreBank_out
        {
            [JsonProperty("checkAccountCoreBank_out")]
            public CheckAccountCoreBank_Out checkAccountCoreBank_out { get; set; }

            public class CheckAccountCoreBank_Out
            {
                [JsonProperty("transactionInfo")]
                public TransactionInfoModel TransactionInfo { get; set; }
            }
        }
    }
}
