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
            byte[] byte_array = Encoding.ASCII.GetBytes(memoryStream);
            _ = SendEmailAsync(reportingManagerEmailId, employeeName, false, false, true, byte_array,fileName+employeeName);
            if (isActivationPdf)
            {
                var sponsorEmailId = _employeeDataOperation.GetSponsorEmailId(sponsorGID);
                _ = SendEmailAsync(sponsorEmailId, employeeName, false, false, true, byte_array,fileName+employeeName);
                _ = SendEmailAsync("1by16cs072@bmsit.in", employeeName, false, false, true, byte_array,fileName+employeeName);
            }
            return true;
        }
        public void SendEmailDeclined(string gId, string employeeName)
        {
            var employeeManagerEmailId = _employeeDataOperation.GetDeactivatedEmployeeEmailId(gId);
            byte[] byte_array = null;
            _ = SendEmailAsync(employeeManagerEmailId, employeeName, false, true, false, byte_array,"");

        }
        public void SendReminderEmail()
        {
            var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            foreach (var employee in employeeDetails)
            {
                DateTime date = Convert.ToDateTime(employee.EmployeeLastWorkingDate.ToString());
                if (DateTime.Today == date || DateTime.Today > date)
                {
                    _ = SendEmailAsync(employee.ReportingManagerEmail, employee.EmployeeName, true, false, false, employee.DeactivationWorkFlowPdfAttachment,"DeactivationWorkflow_"+employee.EmployeeName);
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
                            _ = SendEmailAsync(employee.SponsorEmailID, approvedEmployee.EmployeeName, true, false, false, approvedEmployee.DeactivationWorkFlowPdfAttachment, "DeactivationWorkflow_" + approvedEmployee.EmployeeName);
                        }
                    }
                }
            }
        }
        private async Task SendEmailAsync(string emailId, string employeeName, bool isReminderEmail, bool isDeclinedEmail, bool workFlowInitiatedEmail, byte[] file,string fileName)
        {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("dontreplydeactivationworkflow@gmail.com", "Do not Reply");
                var subject = "";
                var to = new EmailAddress(emailId, "Do not Reply");
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
                if(file!=null)
                {
                    msg.AddAttachment(fileName, Convert.ToBase64String(file));
                
                }
                _ = await client.SendEmailAsync(msg);

        }
    }
}
