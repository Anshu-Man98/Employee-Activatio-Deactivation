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
        [Route("ManagerApproval/PdfDoc")]
        public void PdfDoc(string employeeName, string lastWorkingDatee, string gId, string teamName, string sponsorName, string memoryStream,string reportingManagerEmail)
        {
            _managerAprovalOperation.PdfAttachment(employeeName, lastWorkingDatee, gId, teamName, sponsorName, memoryStream, reportingManagerEmail);
        }


        [HttpGet]
        [Route("ManagerApproval/DeactivationDetails")]
        public JsonResult DeactivationDetails()
        {
            var userEmail = "";
            if (User.Identity.IsAuthenticated)
            {
                userEmail = GetUserEmail(User);
            }

            return Json(_managerAprovalOperation.GetPendingDeactivationWorkflowForParticularManager(userEmail));

        }

        [HttpGet]
        [Route("ManagerApproval/PdfDetails")]
        public ActionResult PdfDetails(string GId)
        {
            byte[] cc =_managerAprovalOperation.Getpdf(GId);
            return Json("data:application/pdf;base64," + Convert.ToBase64String(cc));

        }

        private static string GetUserEmail(ClaimsPrincipal User)
        {
            return (User.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals("preferred_username")).FirstOrDefault().Value);
        }

        [HttpGet]
        [Route("ManagerApproval/RequestApprove")]
        public JsonResult RequestApprove(string gId)
        {
            return Json(_managerAprovalOperation.ApproveRequest(gId));
        }

        [HttpGet]
        [Route("ManagerApproval/RequestDenied")]
        public JsonResult RequestDenied(string gId)
        { 
            return Json(_managerAprovalOperation.DeclineRequest(gId));
        }

    }
}