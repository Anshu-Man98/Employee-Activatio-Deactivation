using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    public class TeamsController : Controller
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;

        public TeamsController(IEmployeeDataOperation employeeDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
        }

        [HttpGet]
        [Route("Teams/GetSponsorDetails")]
        public JsonResult GetSponsorDetails()
        {
            return Json(_employeeDataOperation.RetrieveAllSponsorDetails());
        }
    }
}
