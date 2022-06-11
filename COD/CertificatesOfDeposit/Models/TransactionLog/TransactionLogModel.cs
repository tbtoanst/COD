using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell
{
    public class TransactionLogModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("root_id")]
        public string RootId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("created_user")]
        public string CreatedUser { get; set; }
        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}
