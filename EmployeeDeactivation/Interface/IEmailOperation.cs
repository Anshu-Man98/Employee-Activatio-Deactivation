using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
         Task SendReminderEmail();
        Task SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(EmailDetails details, bool isActivationPdf);
        bool AddMailConfigurationData(/*string ActivationMail, string DeactivationMail, string ReminderMail, string DeclinedMail, */string SendGrid, string EmailTimer);
        //string RetrieveSpecificConfiguration(string key);
        List<Tokens> RetrieveAllMailContent();

    }
}
