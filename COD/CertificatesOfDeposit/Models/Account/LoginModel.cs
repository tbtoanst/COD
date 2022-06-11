using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Models.Account
{
    public class LoginModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class ChangePassModel
    {
        [JsonProperty("password_new")]
        public string PasswordNew { get; set; }

        [JsonProperty("password_old")]
        public string PasswordOld { get; set; }
    }
}
