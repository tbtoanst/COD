using Newtonsoft.Json;

namespace CertificatesOfDeposit.Models.BankPassbook
{
    public class BankPassbookModel
    {
        [JsonProperty("pm_org_id")]
        public string OrgId { get; set; }
        [JsonProperty("pm_inventory_id")]
        public string InventoryId { get; set; }
        [JsonProperty("pm_item_id")]
        public string ItemId { get; set; }
        [JsonProperty("pm_receive_transerial_id")]
        public string ReceiveTranserialId { get; set; }
        [JsonProperty("pm_serial_prefix")]
        public string SerialPrefix { get; set; }
        [JsonProperty("pm_serial_number")]
        public string SerialNumber { get; set; }
    }
}
