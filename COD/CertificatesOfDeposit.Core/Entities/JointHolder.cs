using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class JointHolder
    {

        [Column("cif_dcsh")]
        public string CifDcsh { get; set; }

        [Column("ten_dcsh")]
        public string TenDcsh { get; set; }

    }
}
