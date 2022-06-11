using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.Transaction
{
    public class StepStatusModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("parent_id")]
        public string ParentId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("is_active")]
        public string IsActive { get; set; }
        [JsonProperty("is_delete")]
        public string IsDelete { get; set; }
        [JsonProperty("is_reject")]
        public string IsReject { get; set; }
        [JsonProperty("is_approve")]
        public string IsApprove { get; set; }
        [JsonProperty("phase_code")]
        public string PhaseCode { get; set; }
        [JsonProperty("lane_code")]
        public string LaneCode { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("created_on")]
        public string CreatedOn { get; set; }
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("updated_on")]
        public string UpdatedOn { get; set; }
        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }
    }
}
