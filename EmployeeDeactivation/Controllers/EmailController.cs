using EmployeeDeactivation.Interface;
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
        public JsonResult PdfAttachmentEmail(byte[] pdfFileArray, string employeeName, string teamName, string sponsorGID, bool isActivationPDf)
        {
           return Json(_emailOperation.SendPDfAsEmailAttachment(pdfFileArray, employeeName, teamName, sponsorGID, isActivationPDf));
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
