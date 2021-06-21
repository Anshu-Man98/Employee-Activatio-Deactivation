using EmployeeDeactivation.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    public class EmailController
    {
        private readonly IEmailOperation _emailOperation;

        public EmailController(IEmailOperation emailOperation)
        {
            _emailOperation = emailOperation;
        }
        [HttpPost]
        [Route("Email/PdfAttachmentEmail")]
        public void PdfAttachment(string memoryStream, string employeeName, string teamName, string sponsorGID, bool isActivationPDf)
        {
            _emailOperation.SendPDfAsEmailAttachment(memoryStream, employeeName, teamName, sponsorGID, isActivationPDf);
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
