using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailOperation _emailOperation;

        public EmailController(IEmailOperation emailOperation)
        {
            _emailOperation = emailOperation;
        }
        [HttpPost]
        [Route("Email/PdfAttachmentEmail")]
        public JsonResult PdfAttachmentEmail(byte[] pdfFileArray, string employeeName, string teamName, bool isActivationPDf , string siemensGID)
        {
           return Json(_emailOperation.SendPDfAsEmailAttachment(new EmailDetails() { ActivatedEmployee = new ActivationEmployeeDetails() 
           { ActivationWorkFlowPdfAttachment = pdfFileArray, TeamName = teamName, GId = siemensGID }, EmployeeName = employeeName}, isActivationPDf));
        }

        [HttpGet]
        [Route("Email/SendReminder")]
        public void SendReminder()
        {
            _emailOperation.SendReminderEmail();
        }

        [HttpPost]
        [Route("Email/DeclineEmail")]
        public void DeclineEmail(string gId, string employeeName)
        {
            _emailOperation.SendEmailDeclined(gId, employeeName);
        }
    }
}
