using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Account
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("url_avatar")]
        public string UrlAvatar { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; internal set; }

        [JsonProperty("roles")]
        public List<RoleModel> Roles { get; set; }
    }
}
