using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models
{
    public class PermitModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }
}
