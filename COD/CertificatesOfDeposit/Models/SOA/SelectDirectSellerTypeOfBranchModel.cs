using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class SelectDirectSellerTypeOfBranchModel
    {
        public class SelectDirectSellerTypeOfBranchIn
        {
            [JsonProperty("selectDirectSellerTypeOfBranch_in")]
            public SelectDirectSellerTypeOfBranch_in SelectCustomerRefDataMgmtCIFNum_in { get; set; }
        }

        public class SelectDirectSellerTypeOfBranchOut
        {
            [JsonProperty("selectDirectSellerTypeOfBranch_out")]
            public SelectDirectSellerTypeOfBranch_out SelectCustomerRefDataMgmtCIFNum_out { get; set; }
        }
    }

    public class SelectDirectSellerTypeOfBranch_in
    {
        [JsonProperty("transactionInfo")]
        public TransactionInfoModel TransactionInfo { get; set; }
        [JsonProperty("hrEmpDataInfo")]
        public HrEmpDataInfoModel HrEmpDataInfo { get; set; }
    }

    public class SelectDirectSellerTypeOfBranch_out
    {
        [JsonProperty("transactionInfo")]
        public TransactionInfoModel TransactionInfo { get; set; }
        [JsonProperty("hrEmpDataInfo")]
        public HrEmpDataInfoModel[] HrEmpDataInfo { get; set; }
    }
}
