using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class AccountClassFcc
    {
        [Column("MA_SP")]
        public string MaSp { get; set; }
        [Column("TEN_SP")]
        public string TenSp { get; set; }
        [Column("MA_KYHIEU")]
        public string MaKyHieu { get; set; }
        [Column("MA_ANCHI")]
        public string MaAnChi { get; set; }
    }
}
