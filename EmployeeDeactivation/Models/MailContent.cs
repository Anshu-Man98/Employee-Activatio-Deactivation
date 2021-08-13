using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class MailContent
    {
        [Key]
        public string MailType { get; set; }
        public string MailContents { get; set; }
    }
}
