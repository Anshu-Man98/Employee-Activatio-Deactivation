using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IEmailOperation
    {
        Task SendReminderEmail();
        //Task SendEmailDeclined(string gId, string employeeName);
        bool SendPDfAsEmailAttachment(EmailDetails details, bool isActivationPdf);
        bool AddMailConfigurationData(string SendGrid, string EmailTimer);
        //string RetrieveSpecificConfiguration(string key);
        List<Tokens> RetrieveAllMailContent();
        bool AddMailContentData(string ActivationMailInitiated, string DeactivationMailInitiated, string DeactivationMailLastWorkingDayToSponsor, string DeactivationWorkflowDaysBeforeRemainder, string DeactivationWorkflowToEmployeeRemainder, string ActivationWorkFlowRemainderToManager, string ActivationWorkFlowRemainderToEmployee, string EmailToAssignAUdomainWBT /*, string DeclinedMail, string DeactivationMailLastWorkingDayToManager*/);
       
    }
}
