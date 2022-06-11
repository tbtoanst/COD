using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class UserRole
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("role_id")]
        public string RoleId { get; set; }
    }
}
