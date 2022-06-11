using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class Role
    {
        [Column("role_id")]
        public string Id { get; set; }

        [Column("role_name")]
        public string Name { get; set; }

        [Column("is_actived")]
        public int IsActived { get; set; }

        [Column("description")]
        public string Description { get; set; }

        //[Column("guard_name")]
        //public string GuardName { get; set; }

        //[Column("is_default")]
        //public bool? IsDefault { get; set; }

        //[Column("created_at")]
        //public DateTime CreatedDate { get; set; }

        //[Column("updated_at")]
        //public DateTime? UpdatedDate { get; set; }

        [Column("required_exists_fcc")]
        public int RequiredExistsFcc { get; set; }
    }
}
