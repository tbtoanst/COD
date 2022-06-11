using System.Collections.Generic;

namespace CertificatesOfDeposit.API.Models.Account
{
    public class PositionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public BranchModel Branch { get; internal set; }
        public DepartmentModel Department { get; set; }
    }
}
