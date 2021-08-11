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
        bool AddMailContentData(MailContent mailContent);
        List<MailContent> RetrieveAllMailContent();
        bool AddTokenData(Token token);
        List<Token> RetrieveAllToken();

    }
}
