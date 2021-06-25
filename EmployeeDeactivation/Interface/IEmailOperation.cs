using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
        void SendReminderEmail();
        void SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(string memoryStream, string employeeName, string teamName, string sponsorGID, bool isActivationPdf);
    }
}
