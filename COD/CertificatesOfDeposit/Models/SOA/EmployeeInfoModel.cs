using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.SOA
{
    public class HrEmpDataInfoModel
    {

        [JsonProperty("hrLoaiNhanVien")]
        public string HrLoaiNhanVien { get; set; }
        [JsonProperty("hrMaDonViGoc")]
        public string HrMaDonViGoc { get; set; }
        [JsonProperty("hrMSNV")]
        public string HrMSNV { get; set; }
        [JsonProperty("hrHoTen")]
        public string HrHoTen { get; set; }

        [JsonProperty("hrDisplayHoTen")]
        public string HrDisplayHoTen
        {
            get { return $"{HrMSNV} - {HrHoTen}" ; }
        }

        [JsonProperty("hrDonViGoc")]
        public string HrDonViGoc { get; set; }
    }
}
