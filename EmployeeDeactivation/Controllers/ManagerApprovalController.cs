using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    
    public class ManagerApprovalController : Controller
    {
        private readonly IManagerApprovalOperation _managerAprovalOperation;

        public ManagerApprovalController(IManagerApprovalOperation managerAprovalOperation)
        {
            _managerAprovalOperation = managerAprovalOperation;
        }

        [Authorize("Admin&Manager")]
        public IActionResult ManagerApprovalPage()
        {
            return View();
        }


        [HttpPost]
        [Route("ManagerApproval/AddPendingDeactivationRequestToDatabase")]
        public void AddPendingDeactivationRequestToDatabase(ManagerApprovalStatus managerApprovalStatus)
        {
            _managerAprovalOperation.AddPendingDeactivationRequestToDatabase(managerApprovalStatus);
        }


        [HttpGet]
        [Route("ManagerApproval/GetPendingDeactivationWorkflowForParticularManager")]
        public JsonResult GetPendingDeactivationWorkflowForParticularManager()
        {
            string userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = GetUserEmail(User);
            }
            return Json(_managerAprovalOperation.GetPendingDeactivationWorkflowForParticularManager(userEmail));

        }

        [HttpGet]
        [Route("ManagerApproval/DownloadDeactivationPdf")]
        public ActionResult DownloadDeactivationPdf(string gId)
        {
           return Json("data:application/pdf;base64," + 
               Convert.ToBase64String(_managerAprovalOperation.DownloadDeactivationPdfFromDatabase(gId)));

        }

        [HttpGet]
        [Route("ManagerApproval/AddApprovedDeactivationRequestToDatabase")]
        public JsonResult AddApprovedDeactivationRequestToDatabase(string gId)
        {
            return Json(_managerAprovalOperation.ApproveRequest(gId));
        }

        [HttpGet]
        [Route("ManagerApproval/AddDeniedDeactivationRequestToDatabase")]
        public JsonResult AddDeniedDeactivationRequestToDatabase(string gId)
        { 
            return Json(_managerAprovalOperation.DeclineRequest(gId));
        }
        private static string GetUserEmail(ClaimsPrincipal User)
        {
            return User.Identities.FirstOrDefault().Claims.FirstOrDefault(c => c.Type.Equals("preferred_username")).Value;
        }

    }
}