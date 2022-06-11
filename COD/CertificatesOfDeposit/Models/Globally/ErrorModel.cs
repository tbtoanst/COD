using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Models.Globally
{
    public class ErrorModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; } = new List<object>();

        [JsonProperty("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
