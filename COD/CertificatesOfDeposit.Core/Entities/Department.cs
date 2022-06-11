using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Department
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("branch_id")]
        public string BranchId { get; internal set; }

        [Column("active_flag")]
        public int Level { get; set; }

        [Column("parent_id")]
        public string ParentId { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
