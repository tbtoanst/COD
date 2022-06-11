using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Branch
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("active_flag")]
        public bool IsActive { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
    }
}
