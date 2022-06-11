using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.Account.Request
{
    public class RequestCustomerBySOA
    {
        [JsonProperty("cif")]
        public string CIF { get; set; }

        [JsonProperty("id_number")]
        public string IDNumber { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class RequestEmployeeBySOA
    {
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }
        [JsonProperty("employee_class")]
        public string EmployeeClass { get; set; }
    }

    public class RequestPaymentAccountBySOA
    {
        [JsonProperty("cif")]
        public string CIF { get; set; }
    }

    public class RequestBankPassbookInfo
    {
        [JsonProperty("acc_class")]
        public string AccountClass { get; set; }

        //[JsonProperty("prefix")]
        //public string Prefix { get; set; }
        //[JsonProperty("code")]
        //public string Code { get; set; }
        //[JsonProperty("branch_code")]
        //public string BranchCode { get; set; }
        //[JsonProperty("maker_cc")]
        //public string MakerAcc { get; set; }
    }

    public class RequestBankPassbookPushSerialNo
    {
        //[JsonProperty("prefix")]
        //public string Prefix { get; set; }
        //[JsonProperty("code")]
        //public string Code { get; set; }
        //[JsonProperty("branch_code")]
        //public string BranchCode { get; set; }
        //[JsonProperty("maker_cc")]
        //public string MakerAcc { get; set; }
        [JsonProperty("acc_class")]
        public string AccountClass { get; set; }
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }
    }
}
