using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class WelcomeCard
    {
        public string WelcomeImageData { get; set; }
        public string WelcomeEmployeeFullName { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmail { get; set; }
        public string EmployeeId { get; set; }
    }
}
