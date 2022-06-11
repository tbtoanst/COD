using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.Fcc
{
    public class AccountClassFccModel
    {
        [JsonProperty("ma_sp")]
        public string MaSp { get; set; }
        [JsonProperty("ten_sp")]
        public string TenSp { get; set; }
        [JsonProperty("ma_kyhieu")]
        public string MaKyHieu { get; set; }
        [JsonProperty("ma_anchi")]
        public string MaAnChi { get; set; }
    }
}
