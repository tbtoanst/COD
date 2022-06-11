using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell
{
    public class PartnerModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cif_num")]
        public string CifNum { get; set; }

        [JsonProperty("is_active")]
        public int IsActive { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
