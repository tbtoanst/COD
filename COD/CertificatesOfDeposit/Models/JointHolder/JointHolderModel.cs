using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell
{
    public class JointHolderModel
    {
        [JsonProperty("cif_dcsh")]
        public string CifDcsh { get; set; }

        [JsonProperty("ten_dcsh")]
        public string TenDcsh { get; set; }
    }
}
