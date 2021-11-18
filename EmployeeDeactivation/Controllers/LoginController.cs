using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeDeactivation.Controllers
{
    [Authorize("Admin&Manager")]
    public class LoginController : Controller
    {
        //private readonly ILoginOperation _loginOperation;
        //public LoginController(ILoginOperation loginOperation)
        //{
        //    _loginOperation = loginOperation;
        //}

        public IActionResult LoginPage()
        {
            return View();
        }
    }
}
