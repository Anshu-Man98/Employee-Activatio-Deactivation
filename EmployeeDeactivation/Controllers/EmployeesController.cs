using Microsoft.AspNetCore.Mvc;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Authorization;
using EmployeeDeactivation.Models;

namespace EmployeeDeactivation.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;

        public EmployeesController(IEmployeeDataOperation employeeDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
        }

        [Authorize(Roles = "Manager,Admin")]
        [Authorize("Admin&Manager")]

        [HttpGet]
        public IActionResult EmployeeActivationForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeDeactivationForm()
        {
            return View();
        }

        [HttpPost]
        [Route("Employees/AddDetailsToDatabase")]
        public JsonResult AddDetailsToDatabase(EmployeeDetails employeeDetails,bool isDeactivatedWorkFlow)
        {
            return Json( _employeeDataOperation.AddEmployeeData(employeeDetails, isDeactivatedWorkFlow));
        }
        [HttpPost]
        [Route("Employees/AddActivationPdfToDatabase")]
        public JsonResult AddActivationPdfToDatabase(byte[] pdf, string gId)
        {
            return Json(_employeeDataOperation.SavePdfToDatabase(pdf, gId));
        }
    }
}
