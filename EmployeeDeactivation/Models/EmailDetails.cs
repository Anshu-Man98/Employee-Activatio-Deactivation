using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class EmailDetails
    {
        public string FromEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string CcEmailId { get; set; }
        public TypeOfWorkflow TypeOfWorkflow { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public ActivationEmployeeDetails ActivatedEmployee { get; set; }
    }
}
