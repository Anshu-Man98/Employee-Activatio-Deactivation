using System;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
        void SendReminderEmail();
        void SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(byte[] pdfFileArray, string employeeName, string teamName, string sponsorGID, bool isActivationPdf);
    }
}
