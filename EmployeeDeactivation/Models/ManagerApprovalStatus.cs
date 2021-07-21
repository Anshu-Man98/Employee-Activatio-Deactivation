using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class ManagerApprovalStatus
    {
        public string EmployeeName { get; set; }
        public string EmployeeLastWorkingDate { get; set; }
        [Key]
        public string EmployeeGId { get; set; }
        public string EmployeeTeamName { get; set; }
        public string SponsorName { get; set; }
        public byte[] DeactivationWorkFlowPdfAttachment { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string WorkFlowStatus { get; set; }
    }
}
