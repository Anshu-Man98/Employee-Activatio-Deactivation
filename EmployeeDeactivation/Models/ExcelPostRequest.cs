using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class ExcelPostRequest
    {
        public string index { get; set; }
        public string[][] values { get; set; }
    }
}
