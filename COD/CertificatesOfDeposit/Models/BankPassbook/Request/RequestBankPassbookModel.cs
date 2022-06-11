using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.BankPassbook.Request
{
    public class RequestBankPassbookModel
    {
        [JsonProperty("pm_gdv")]
        public string GDV { get; set; }
        [JsonProperty("pm_sokyhieu")]
        public string SoKyHieu { get; set; }
        [JsonProperty("pm_machinhanh")]
        public string MaChiNhanh { get; set; }
        [JsonProperty("pm_maanchi")]
        public string MaAnChi { get; set; }
        
        [JsonProperty("pm_serial_number")]
        public string SerialNumber { get; set; }
    }
}
