using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class StepStatus
    {
        [Column("id")]
        public string ID { get; set; }
        [Column("parent_id")]
        public string ParentId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("is_active")]
        public string IsActive { get; set; }
        [Column("is_delete")]
        public string IsDelete { get; set; }
        [Column("is_reject")]
        public string IsReject { get; set; }
        [Column("is_approve")]
        public string IsApprove { get; set; }
        [Column("phase_code")]
        public string PhaseCode { get; set; }
        [Column("lane_code")]
        public string LaneCode { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("created_on")]
        public string CreatedOn { get; set; }
        [Column("created_by")]
        public string CreatedBy { get; set; }
        [Column("updated_on")]
        public string UpdatedOn { get; set; }
        [Column("updated_by")]
        public string UpdatedBy { get; set; }
    }
}
