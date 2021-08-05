using EmployeeDeactivation.Models;
using System;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
        void SendReminderEmail();
        void SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(EmailDetails details, bool isActivationPdf);
    }
}
