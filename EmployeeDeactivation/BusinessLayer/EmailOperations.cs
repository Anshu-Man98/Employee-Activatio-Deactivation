using EmployeeDeactivation.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
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
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(memoryStream));
            SendGrid.Helpers.Mail.Attachment file = new SendGrid.Helpers.Mail.Attachment
            {
                Filename = fileName + employeeName + ".pdf",
                Content = Encoding.UTF8.GetString((stream).ToArray()),
                Type = "application/pdf",
                ContentId = "ContentId"
            };

            _ = SendEmail(reportingManagerEmailId, employeeName, false, false, true, file);
            if (isActivationPdf)
            {
                var sponsorEmailId = _employeeDataOperation.GetSponsorEmailId(sponsorGID);
                _ = SendEmail(sponsorEmailId, employeeName, false, false, true, file);
                _ = SendEmail("1by16cs072@bmsit.in", employeeName, false, false, true, file);
            }
            return true;
        }
        public void SendEmailDeclined(string gId, string employeeName)
        {
            var employeeManagerEmailId = _employeeDataOperation.GetDeactivatedEmployeeEmailId(gId);
            SendGrid.Helpers.Mail.Attachment file = null;
            _ = SendEmail(employeeManagerEmailId, employeeName, false, true, false, file);

        }
        public void SendReminderEmail()
        {
            var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            foreach (var employee in employeeDetails)
            {
                DateTime date = Convert.ToDateTime(employee.EmployeeLastWorkingDate.ToString());
                if (DateTime.Today == date || DateTime.Today > date)
                {
                    MemoryStream stream = new MemoryStream(employee.DeactivationWorkFlowPdfAttachment);
                    SendGrid.Helpers.Mail.Attachment file = new SendGrid.Helpers.Mail.Attachment
                    {
                        Filename = "Deactivation workflow_" + employee.EmployeeName + ".pdf",
                        Content = Encoding.UTF8.GetString((stream).ToArray()),
                        Type = "application/pdf",
                        ContentId = "ContentId"
                    };
                    _ = SendEmail(employee.ReportingManagerEmail, employee.EmployeeName, true, false, false, file);
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
                            MemoryStream stream = new MemoryStream(approvedEmployee.DeactivationWorkFlowPdfAttachment);
                            SendGrid.Helpers.Mail.Attachment file = new SendGrid.Helpers.Mail.Attachment
                            {
                                Filename = "Deactivation workflow_" + approvedEmployee.EmployeeName + ".pdf",
                                Content = Encoding.UTF8.GetString((stream).ToArray()),
                                Type = "application/pdf",
                                ContentId = "ContentId"
                            };
                            _ = SendEmail(employee.SponsorEmailID, approvedEmployee.EmployeeName, true, false, false, file);
                        }
                    }
                }
            }
        }
        private async Task SendEmail(string emailId, string employeeName, bool isReminderEmail, bool isDeclinedEmail, bool workFlowInitiatedEmail, SendGrid.Helpers.Mail.Attachment file)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sonalisingh7639@gmail.com", "Example User");
            var subject = "";
            var to = new EmailAddress(emailId, "Example User");
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
            msg.AddAttachment(file);
            _ = await client.SendEmailAsync(msg);

        }
        
    }
}
