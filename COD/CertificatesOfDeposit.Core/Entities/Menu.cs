using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Menu
    {
        [Column("menu_id")]
        public string Id { get; set; }

        [Column("menu_parent_id")]
        public string ParentId { get; set; }

        [Column("menu_name")]
        public string Name { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("is_active")]
        public int IsActive { get; set; }

        [Column("order_no")]
        public int OrderNo { get; set; }

        [Column("icon")]
        public string Icon { get; set; }
    }
}
