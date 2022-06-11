using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Buy
{
    public class BuyContractModel
    {
        [JsonProperty("buy_id")]
        public string BuyID { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("buy_file")]
        public byte[] BuyFile { get; set; }
        [JsonProperty("created_user")]
        public string CreatedUser { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_user")]
        public string UpdatedUser { get; set; }
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty("approved_user")]
        public string ApprovedUser { get; set; }
        [JsonProperty("approved_at")]
        public DateTime? ApprovedAt { get; set; }
        [JsonProperty("file_name")]
        public string FileName { get; set; }
        [JsonProperty("file_type")]
        public string FileType { get; set; }
    }
}
