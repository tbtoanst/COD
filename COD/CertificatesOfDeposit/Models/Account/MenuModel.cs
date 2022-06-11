using CertificatesOfDeposit.Core.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Account
{
    public class MenuModel
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("menus")]
        public List<MenuModel> Menus { get; set; } = new List<MenuModel>();

        [JsonProperty("permit")]
        public List<PermitModel> Permits { get; internal set; }
    }
}
