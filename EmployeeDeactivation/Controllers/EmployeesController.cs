using System;
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

        [Authorize(Roles ="Manager,Admin")]
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
        public JsonResult AddDetails(EmployeeDetails employeeDetails,bool isDeactivatedWorkFlow)
        {
            return Json( _employeeDataOperation.AddEmployeeData(employeeDetails, isDeactivatedWorkFlow));
        }
    }
}
