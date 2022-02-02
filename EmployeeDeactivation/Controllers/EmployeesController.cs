using Microsoft.AspNetCore.Mvc;
using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Authorization;
using EmployeeDeactivation.Models;

namespace EmployeeDeactivation.Controllers
{
    [Authorize("Admin&Manager")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;

        public EmployeesController(IEmployeeDataOperation employeeDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
        }


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
        public JsonResult AddDetailsToDatabase(EmployeeDetails employeeDetails)
        {
            return Json( _employeeDataOperation.AddEmployeeData(employeeDetails));
        }
        [HttpPost]
        [Route("Employees/AddActivationPdfToDatabase")]
        public JsonResult AddActivationPdfToDatabase(byte[] pdf, string gId)
        {
            return Json(_employeeDataOperation.SavePdfToDatabase(pdf, gId));
        }
        [HttpPost]
        [Route("Employees/DeleteDeactivationDetails")]
        public JsonResult DeleteDeactivationDetails(string gId)
        {
            return Json(_employeeDataOperation.DeleteDeactivationDetails(gId));
        }

        [HttpPost]
        [Route("Employees/DeleteActivationDetails")]
        public JsonResult DeleteActivationDetails(string gId)
        {
            return Json(_employeeDataOperation.DeleteActivationDetails(gId));
        }
    }
}
