using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeDeactivation.Models
{
    public class Teams
    {
        
        public string TeamName { get; set; }
        public string SponsorFirstName { get; set; }
        public string SponsorLastName { get; set; }
        public string SponsorEmailID { get; set; }
        [Key]
        public string SponsorGID { get; set; }
        public string Department { get; set; }
        public string ReportingManagerEmailID { get; set; }
        public string SivantosPointEmailID { get; set; }
        public string CmEmailID { get; set; }
        public string CcEmailID { get; set; }
    }
}
