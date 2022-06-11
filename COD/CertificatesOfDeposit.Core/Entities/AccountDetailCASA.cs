using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class AccountDetailCASA
    {
        [Column("cust_ac_no")]
        public string custAcNo { get; set; }
        [Column("ccy")]
        public string Ccy { get; set; }
        [Column("ac_desc")]
        public string acDesc { get; set; }
        [Column("account_class")]
        public string accountClass { get; set; }
        [Column("cust_no")]
        public string custNo { get; set; }
        [Column("act_type")]
        public string actType { get; set; }
        [Column("current_balance")]
        public string currentBalance { get; set; }
        [Column("trang_thai")]
        public string trangThai { get; set; } 
    }
}
