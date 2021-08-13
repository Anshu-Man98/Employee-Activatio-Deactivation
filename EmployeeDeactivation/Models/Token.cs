using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class Token
    {
        [Key]
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
    }
}
