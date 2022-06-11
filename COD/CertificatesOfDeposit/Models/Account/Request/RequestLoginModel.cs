using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.Account.Request
{
    public class RequestLoginModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
