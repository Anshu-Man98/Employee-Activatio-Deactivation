using EmployeeDeactivation.Interface;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

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
        public void SendPDfAsEmailAttachment(string memoryStream, string employeeName, string teamName, string sponsorGID, bool isActivationPdf)
        {
            var fileName = isActivationPdf ? "Activation workflow_" : "Deactivation workflow_";
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(memoryStream));
            Attachment file = new Attachment(stream, fileName + employeeName + ".pdf", "application/pdf");
            SendEmail(reportingManagerEmailId, employeeName, false, false, true, file);
            if (isActivationPdf)
            {
                var sponsorEmailId = _employeeDataOperation.GetSponsorEmailId(sponsorGID);
                SendEmail(sponsorEmailId, employeeName, false, false, true, file);
                SendEmail("1by16cs072@bmsit.in", employeeName, false, false, true, file);
            }
        }
        public void SendEmailDeclined(string gId, string employeeName)
        {
            var employeeManagerEmailId = _employeeDataOperation.GetDeactivatedEmployeeEmailId(gId);
            Attachment file = null;
            SendEmail(employeeManagerEmailId, employeeName, false, true, false, file);

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
                    Attachment file = new Attachment(stream, "Deactivation workflow_" + employee.EmployeeName + ".pdf", "application/pdf");
                    SendEmail(employee.ReportingManagerEmail, employee.EmployeeName, true, false, false, file);
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
                            Attachment file = new Attachment(stream, "Deactivation workflow_" + approvedEmployee.EmployeeName + ".pdf", "application/pdf");
                            SendEmail(employee.SponsorEmailID, approvedEmployee.EmployeeName, true, false, false, file);
                        }
                    }
                }
            }
        }
        private void SendEmail(string emailId, string employeeName, bool isReminderEmail, bool isDeclinedEmail, bool workFlowInitiatedEmail, Attachment file)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress("dontreplydeactivationworkflow@gmail.com"),
                Sender = new MailAddress("dontreplydeactivationworkflow@gmail.com")
            };
            message.To.Add(emailId);
            if (isDeclinedEmail)
            {
                message.Subject = "Deactivation workflow declined";
                message.Body = employeeName + " your account deactivation form has been declined";

            }
            if (workFlowInitiatedEmail)
            {
                message.Subject = "Workflow initiated";
                message.Attachments.Add(file);
            }
            if (isReminderEmail)
            {
                message.Subject = "Deactivation workflow";
                message.Body = "Today is " + employeeName + "'s last working day please check if you have approved the deactivation workflow";
                message.Attachments.Add(file);
            }

            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dontreplydeactivationworkflow@gmail.com", "Siemens@Banglore98"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtp.Send(message);

        }
        
    }
}
