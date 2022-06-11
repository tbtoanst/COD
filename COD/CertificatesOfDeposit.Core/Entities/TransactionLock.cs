using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class TransactionLock
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("updated_user")]
        public string UpdatedUser { get; set; }
        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }
    }
}
