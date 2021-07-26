using EmployeeDeactivation.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmailOperations:IEmailOperation
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly IManagerApprovalOperation _managerApprovalOperation;

        public EmailOperations(IEmployeeDataOperation employeeDataOperation, IManagerApprovalOperation managerApprovalOperation)
        {
            _employeeDataOperation = employeeDataOperation;
            _managerApprovalOperation = managerApprovalOperation;
        }
        public bool SendPDfAsEmailAttachment(string memoryStream, string employeeName, string teamName, string sponsorGID, bool isActivationPdf)
        {

            var fileName = isActivationPdf ? "Activation workflow_" : "Deactivation workflow_";
            var emailDetails = _employeeDataOperation.GetReportingEmailIds(teamName);
            byte[] byte_array = Encoding.ASCII.GetBytes(memoryStream);
            if (!isActivationPdf)
            {
                _ = SendEmailAsync(emailDetails[3], emailDetails[0], emailDetails[2], employeeName, false, false, true, byte_array, fileName + employeeName);
            }
            if (isActivationPdf)
            {
                _ = SendEmailAsync(emailDetails[0], emailDetails[1], emailDetails[2], employeeName, false, false, true, byte_array,fileName+employeeName);
            }
            return true;
        }
        public void SendEmailDeclined(string gId, string employeeName)
        {
            var employeeDetails = _employeeDataOperation.GetDeactivatedEmployeeDetails(gId);
            var reportingManagerEmailId = _employeeDataOperation.GetReportingEmailIds(employeeDetails[1])[3];
            byte[] byte_array = null;
            _ = SendEmailAsync(reportingManagerEmailId, employeeDetails[0], "", employeeName, false, true, false, byte_array,"");

        }
        public void SendReminderEmail()
        {
            var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            foreach (var employee in employeeDetails)
            {
                DateTime date = Convert.ToDateTime(employee.EmployeeLastWorkingDate.ToString());
                if (DateTime.Today == date || DateTime.Today > date)
                {
                    var emailDetails = _employeeDataOperation.GetReportingEmailIds(employee.EmployeeTeamName);
                    _ = SendEmailAsync(emailDetails[0], employee.ReportingManagerEmail,"", employee.EmployeeName, true, false, false, employee.DeactivationWorkFlowPdfAttachment,"DeactivationWorkflow_"+employee.EmployeeName);
                }
            }
            var approvedEmployeeDetails = _managerApprovalOperation.GetAllApprovedDeactivationWorkflows();
            var employeeData = _employeeDataOperation.RetrieveAllDeactivatedEmployees();
            foreach (var approvedEmployee in approvedEmployeeDetails)
            {
                if (DateTime.Today.ToString() == approvedEmployee.EmployeeLastWorkingDate)
                {
                    foreach (var employee in employeeData)
                    {
                        if (employee.GId == approvedEmployee.EmployeeGId)
                        {
                            _ = SendEmailAsync("arun",employee.SponsorEmailID, _employeeDataOperation.GetDeactivatedEmployeeDetails(employee.GId)[3], approvedEmployee.EmployeeName, true, false, false, approvedEmployee.DeactivationWorkFlowPdfAttachment, "DeactivationWorkflow_" + approvedEmployee.EmployeeName);
                        }
                    }
                }
            }
        }
        private async Task SendEmailAsync(string fromEmailId,string toEmailId, string ccEmailId,string employeeName, bool isReminderEmail, bool isDeclinedEmail, bool workFlowInitiatedEmail, byte[] file,string fileName)
        {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmailId, "CM Siemens");
                var subject = "";
                var to = new EmailAddress(toEmailId, "");
                var cc = ccEmailId.Split(",");
            List<EmailAddress> emailAddress = new List<EmailAddress>();
            foreach (var item in cc)
            {
                emailAddress.Add(new EmailAddress(item));

            }
                var plainTextContent = "";
                var htmlContent = "<strong>Workflow</strong>";
                if (isDeclinedEmail)
                {
                    subject = "Deactivation workflow declined";
                    plainTextContent = employeeName + " your account deactivation form has been declined";

                }
                if (workFlowInitiatedEmail)
                {
                    subject = "Workflow initiated";
                }
                if (isReminderEmail)
                {
                    subject = "Deactivation workflow";
                    plainTextContent = "Today is " + employeeName + "'s last working day please check if you have approved the deactivation workflow";
                }
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
               msg.AddCcs(emailAddress);
                if(file!=null)
                {
                    msg.AddAttachment(fileName, Convert.ToBase64String(file));
                
                }
                _ = await client.SendEmailAsync(msg);

        }
    }
}
