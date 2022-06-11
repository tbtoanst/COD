using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class SellContract
    {
        [Column("sell_id")]
        public string SellID { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("sell_file")]
        public byte[] SellFile { get; set; }
        [Column("created_user")]
        public string CreatedUser { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_user")]
        public string UpdatedUser { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("approved_user")]
        public string ApprovedUser { get; set; }
        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("file_type")]
        public string FileType { get; set; }
    }
}
