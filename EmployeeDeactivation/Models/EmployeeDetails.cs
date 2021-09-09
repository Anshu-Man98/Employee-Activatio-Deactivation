using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class EmployeeDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        [Key]
        public string GId { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string TeamName { get; set; }
        public string SponsorFirstName { get; set; }
        public string SponsorLastName { get; set; }
        public string SponsorEmailID { get; set; }
        public string SponsorDepartment { get; set; }
        public string SponsorGId { get; set; }
        public string ToEmailId { get; set; }
        public string FromEmailId { get; set; }
        public string CcEmailId { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Nationality { get; set; }
        public byte[] ActivationWorkFlowPdfAttachment { get; set; }
        public bool isDeactivatedWorkFlow { get; set; }
        public string ActivationDate { get; set; }
    }
}
