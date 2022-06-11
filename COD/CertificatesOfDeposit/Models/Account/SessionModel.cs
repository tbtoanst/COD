using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Models.Account
{
    public class SessionModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string DeptpartmentId { get; set; }
        public string DeptpartmentCode { get; set; }
        public string DeptpartmentName { get; set; }
        public string BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchCodeFCC { get; set; }
        public string BranchNameFCC { get; set; }
        public List<string> Roles { get; set; }
    }
}
