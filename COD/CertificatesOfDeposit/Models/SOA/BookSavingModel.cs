using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class ErpInfo
    {
        [JsonProperty("erpPrefix")]
        public string Prefix { get; set; }
        [JsonProperty("erpCode")]
        public string Code { get; set; }
        [JsonProperty("erpBranchCode")]
        public ErpBranchCode BranchCode { get; set; }
        [JsonProperty("Serial")]
        public string Serial { get; set; }
        [JsonProperty("erpReturnTranSerialID")]
        public string ReturnTranSerialID { get; set; }
        [JsonProperty("erpInventoryID")]
        public string InventoryID { get; set; }
        [JsonProperty("erpItemID")]
        public string ItemID { get; set; }
        [JsonProperty("erpOrgID")]
        public string OrgID { get; set; }
    }

    public class ErpBranchCode
    {
        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }
    }

    public class CoreBankAccount
    {
        [JsonProperty("makerAccount")]
        public string MakerAccount { get; set; }
        [JsonProperty("userAccount")]
        public string UserAccount { get; set; }
        [JsonProperty("userFullName")]
        public string UserFullName { get; set; }
        [JsonProperty("branchInfo")]
        public BranchInfoModel BranchInfo { get; set; }
    }
}
