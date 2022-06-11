using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Account
{
    public class PermissionModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("guard_name")]
        public string GuardName { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedDate { get; set; }
        [JsonProperty("permissions")]
        public List<RoleModel> Permissions { get; internal set; }
    }
}
