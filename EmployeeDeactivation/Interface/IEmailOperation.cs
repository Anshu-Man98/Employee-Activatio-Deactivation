using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
        void SendReminderEmail();
        void SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(EmailDetails details, bool isActivationPdf);
        bool AddMailConfigurationData(string ActivationMail, string DeactivationMail, string ReminderMail, string DeclinedMail, string SendGrid, string EmailTimer);
        //string RetrieveSpecificConfiguration(string key);
        List<Tokens> RetrieveAllMailContent();

    }
}
