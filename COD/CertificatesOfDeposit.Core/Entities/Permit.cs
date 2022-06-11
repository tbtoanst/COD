using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Permit
    {
        [Column("permit_id")]
        public string Id { get; set; }

        [Column("permit_parent_id")]
        public string ParentId { get; set; }

        [Column("permit_name")]
        public string Name { get; set; }

        [Column("permit_action")]
        public string Action { get; set; }

        [Column("is_actived")]
        public int IsActived { get; set; }
    }
}
