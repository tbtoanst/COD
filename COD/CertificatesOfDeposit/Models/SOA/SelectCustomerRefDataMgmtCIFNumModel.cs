using Newtonsoft.Json;
using System;
namespace CertificatesOfDeposit.Models.SOA
{
    public class SelectCustomerRefDataMgmtCIFNumModel
    {

        public class SelectCustomerRefDataMgmtCIFNumIn
        {
            [JsonProperty("selectCustomerRefDataMgmtCIFNum_in")]
            public SelectCustomerRefDataMgmtCIFNum_In SelectCustomerRefDataMgmtCIFNum_in { get; set; }
        }

        public class SelectCustomerRefDataMgmtCIFNum_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("CIFInfo")]
            public CifInfoModel CIFInfo { get; set; }
            [JsonProperty("customerInfo")]
            public CustomerInfoModel CustomerInfo { get; set; }
        }


        public class SelectCustomerrefDataMgmtCifNumOut
        {
            [JsonProperty("selectCustomerRefDataMgmtCIFNum_out")]
            public SelectCustomerRefDataMgmtCIFNum_out SelectCustomerRefDataMgmtCIFNum_out { get; set; }
        }

        public class SelectCustomerRefDataMgmtCIFNum_out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("customerList")]
            public CustomerlistModel[] CustomerList { get; set; }
        }
    }
}
