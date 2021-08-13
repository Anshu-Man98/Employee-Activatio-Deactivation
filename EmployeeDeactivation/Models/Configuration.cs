using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class Configuration
    {
        [Key]
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }

    }
}
