using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class ActivationStatus
    {
        [Key]
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UpdateConsolidateIoTResourcesList { get; set; }
        public string BirthdayListUpdated { get; set; }
        public string EmailIdAddedToTeamDL { get; set; }
        public string MemberAddedToTimesheet { get; set; }
        public string InitiateHardwareShipmentRequest { get; set; }
        public string MemberDetailsUpdatedInEDMTTool { get; set; }
        public string InductionPlanAssignment { get; set; }
        public string InductionTrainingRecordUpdated { get; set; }
        public string VivekEcampusListUpdated { get; set; }
        public string ReportingManagerEmail { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime TimerDate { get; set; }
    }
}
