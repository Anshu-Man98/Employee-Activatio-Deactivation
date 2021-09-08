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
        private readonly IManagerApprovalOperation _managerApprovalOperation;

        public ManagerApprovalController(IManagerApprovalOperation managerApprovalOperation)
        {
            _managerApprovalOperation = managerApprovalOperation;
        }

        [Authorize("Admin&Manager")]
        public IActionResult ManagerApprovalPage()
        {
            return View();
        }

        public IActionResult DeactivationStatus()
        {
            return View();
        }

        public IActionResult ActivationStatus()
        {
            return View();
        }


        [HttpPost]
        [Route("ManagerApproval/AddPendingDeactivationRequestToDatabase")]
        public void AddPendingDeactivationRequestToDatabase(ManagerApprovalStatus managerApprovalStatus)
        {
            _managerApprovalOperation.AddPendingDeactivationRequestToDatabase(managerApprovalStatus);
        }

        [HttpPost]
        [Route("ManagerApproval/AddDeactivationTaskToDatabase")]
        public JsonResult AddDeactivationTaskToDatabase(DeactivationStatus deactivationStatus)
        {
           return Json(_managerApprovalOperation.AddDeactivationTaskToDatabase(deactivationStatus));
        }

        [HttpGet]
        [Route("ManagerApproval/AllDeactivationTaskStatus")]
        public JsonResult AllDeactivationTaskStatus()
        {
            return Json(_managerApprovalOperation.RetrieveDeactivationTasks());
        }

        //[HttpGet]
        //[Route("ManagerApproval/AllActivationTaskStatus")]
        //public JsonResult AllActivationTaskStatus()
        //{
        //    return Json(_managerApprovalOperation.RetrieveDeactivationTasks());
        //}


        [HttpGet]
        [Route("ManagerApproval/GetPendingDeactivationWorkflowForParticularManager")]
        public JsonResult GetPendingDeactivationWorkflowForParticularManager()
        {
            string userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = GetUserEmail(User);
            }
            return Json(_managerApprovalOperation.GetPendingDeactivationWorkflowForParticularManager(userEmail));

        }

        [HttpGet]
        [Route("ManagerApproval/DownloadDeactivationPdf")]
        public ActionResult DownloadDeactivationPdf(string gId)
        {
           return Json("data:application/pdf;base64," + 
               Convert.ToBase64String(_managerApprovalOperation.DownloadDeactivationPdfFromDatabase(gId)));

        }

        [HttpGet]
        [Route("ManagerApproval/AddApprovedDeactivationRequestToDatabase")]
        public JsonResult AddApprovedDeactivationRequestToDatabase(string gId)
        {
            return Json(_managerApprovalOperation.ApproveRequest(gId));
        }

        [HttpGet]
        [Route("ManagerApproval/AddDeniedDeactivationRequestToDatabase")]
        public JsonResult AddDeniedDeactivationRequestToDatabase(string gId)
        { 
            return Json(_managerApprovalOperation.DeclineRequest(gId));
        }
        private static string GetUserEmail(ClaimsPrincipal User)
        {
            return User.Identities.FirstOrDefault().Claims.FirstOrDefault(c => c.Type.Equals("preferred_username")).Value;
        }

    }
}