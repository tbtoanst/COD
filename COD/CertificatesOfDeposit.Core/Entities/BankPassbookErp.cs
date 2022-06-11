using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class BankPassbookErp
    {
        [Column("pm_org_id")]
        public int? OrgId { get; set; }
        [Column("pm_inventory_id")]
        public int? InventoryId { get; set; }
        [Column("pm_item_id")]
        public int? ItemId { get; set; }
        [Column("pm_receive_transerial_id")]
        public int? ReceiveTranserialId { get; set; }
        [Column("pm_serial_prefix")]
        public string SerialPrefix { get; set; }
        [Column("pm_serial_number")]
        public string SerialNumber { get; set; }
    }
}
