using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class DeactivationStatus
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TimesheetApproval { get; set; }
        public string EmployeeRemovedFromDLEmailList { get; set; }
        public string HardwaresCollected { get; set; }
        public string RaisedWindowsDeactivationRequestNexus { get; set; }
        public DateTime TimerDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string EmployeeEmail { get; set; }
        public string FromEmail { get; set; }

    }
}
