using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.SOA
{
    public class RetrieveCustomerRefDataMgmtModel
    {

        public class RetrieveCustomerRefDataMgmt_in
        {
            [JsonProperty("retrieveCustomerRefDataMgmt_in")]
            public RetrievecustomerrefdataMgmt_In retrieveCustomerRefDataMgmt_in { get; set; }
        }

        public class RetrievecustomerrefdataMgmt_In
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("CIFInfo")]
            public CifInfoModel CIFInfo { get; set; }
        }

        public class RetrieveCustomerRefDataMgmt_out
        {
            [JsonProperty("retrieveCustomerRefDataMgmt_out")]
            public Retrievecustomerrefdatamgmt_Out retrieveCustomerRefDataMgmt_out { get; set; }
        }

        public class Retrievecustomerrefdatamgmt_Out
        {
            [JsonProperty("transactionInfo")]
            public TransactionInfoModel TransactionInfo { get; set; }
            [JsonProperty("CIFInfo")]
            public CifInfoModel CIFInfo { get; set; }
            [JsonProperty("customerInfo")]
            public CustomerInfoModel CustomerInfo { get; set; }
        }
    }
}
