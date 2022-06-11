using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Partner
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("cif_num")]
        public string CifNum { get; set; }

        [Column("is_active")]
        public int IsActive { get; set; }

        [Column("type")]
        public string Type { get; set; }
    }
}
