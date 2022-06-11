using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class TransactionLog
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("root_id")]
        public string RootId { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("status")]
        public string Sattus { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt   { get; set; }
        [Column("created_user")]
        public string CreatedUser   { get; set; }
        [Column("data")]
        public dynamic Data { get; set; }

    }
}
