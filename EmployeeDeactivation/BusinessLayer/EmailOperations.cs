using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
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
       // public bool SendPDfAsEmailAttachment(byte[] pdfFileArray, string employeeName, string teamName, string sponsorGID, bool isActivationPdf)
         public bool SendPDfAsEmailAttachment(EmailDetails details,bool isActivationPdf)
        {
            var fileName = isActivationPdf ? "Activation workflow_" : "Deactivation workflow_";
            var emailDetails = _employeeDataOperation.GetReportingEmailIds(details.ActivatedEmployee.TeamName);
            

            if (!isActivationPdf)
            {
                details.FromEmailId = emailDetails[3];
                details.ToEmailId = emailDetails[0];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
                //_ = SendEmailAsync(emailDetails[3], emailDetails[0], emailDetails[2], employeeName, TypeOfWorkflow.Deactivation, pdfFileArray, fileName + employeeName, teamName);

                _ = SendEmailAsync(details, TypeOfWorkflow.Deactivation);

            }
            if (isActivationPdf)
            {
                details.FromEmailId = emailDetails[0];
                details.ToEmailId = emailDetails[1];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
                //_ = SendEmailAsync(emailDetails[0], emailDetails[1], emailDetails[2], employeeName, TypeOfWorkflow.Activation, pdfFileArray, fileName+employeeName, teamName);
                _ = SendEmailAsync(details, TypeOfWorkflow.Activation);
            }
            return true;
        }
        public void SendEmailDeclined(string gId, string employeeName)
        {
            var employeeDetails = _employeeDataOperation.GetDeactivatedEmployeeDetails(gId);
            var reportingManagerEmailId = _employeeDataOperation.GetReportingEmailIds(employeeDetails[1])[3];
            byte[] byte_array = null;
            EmailDetails details = new EmailDetails()
            {
                FromEmailId = reportingManagerEmailId,
                ToEmailId = employeeDetails[0],
                CcEmailId = "",
                EmployeeName = employeeName,
                ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = byte_array, TeamName = String.Empty },
                FileName = String.Empty
            };
           // _ = SendEmailAsync(reportingManagerEmailId, employeeDetails[0], "", employeeName,TypeOfWorkflow.DeclinedEmail, byte_array,string.Empty,string.Empty);
            _ = SendEmailAsync(details, TypeOfWorkflow.DeclinedEmail);

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
                    EmailDetails details = new EmailDetails()
                    {
                        FromEmailId = emailDetails[0],
                        ToEmailId = employee.ReportingManagerEmail,
                        CcEmailId = "",
                        EmployeeName = employee.EmployeeName,
                        ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = employee.DeactivationWorkFlowPdfAttachment, TeamName = String.Empty },
                        FileName = "DeactivationWorkflow_" + employee.EmployeeName
                    };
                    //_ = SendEmailAsync(emailDetails[0], employee.ReportingManagerEmail,"", employee.EmployeeName, TypeOfWorkflow.ReminderEmail, employee.DeactivationWorkFlowPdfAttachment,"DeactivationWorkflow_"+employee.EmployeeName,string.Empty);
                    _ = SendEmailAsync(details, TypeOfWorkflow.ReminderEmail);
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
                        if (employee.GId.ToLower() == approvedEmployee.EmployeeGId.ToLower())
                        {
                            EmailDetails details = new EmailDetails()
                            {
                                FromEmailId = employee.FromEmailId,
                                ToEmailId = employee.SponsorEmailID,
                                CcEmailId = _employeeDataOperation.GetDeactivatedEmployeeDetails(employee.GId)[3],
                                EmployeeName = approvedEmployee.EmployeeName,
                                ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = approvedEmployee.DeactivationWorkFlowPdfAttachment, TeamName = String.Empty },
                                FileName = "DeactivationWorkflow_" + approvedEmployee.EmployeeName
                            };
                            //_ = SendEmailAsync("arun",employee.SponsorEmailID, _employeeDataOperation.GetDeactivatedEmployeeDetails(employee.GId)[3], approvedEmployee.EmployeeName,TypeOfWorkflow.ReminderEmail, approvedEmployee.DeactivationWorkFlowPdfAttachment, "DeactivationWorkflow_" + approvedEmployee.EmployeeName,string.Empty);
                            _ = SendEmailAsync(details , TypeOfWorkflow.ReminderEmail);
                        }
                    }
                }
            }
        }
        private async Task SendEmailAsync(EmailDetails details, Enum typeOfWorkflow)
        {
            var apiKey = "S:G:.:B:n:R:E:a:U:W:0:R:C:q:v:F:3:h:v:W:h:g:x:kA.sy38sl:c:0:xgg:sU0msXLd:o0:2:h:RNGcbpeB1g6:T:v:jEtPJk0";
            apiKey = apiKey.Replace(":", "");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(details.FromEmailId, "CM Siemens");
            var subject = "";
            var to = new EmailAddress(details.ToEmailId, "");
            var cc = details.CcEmailId.Split(",");
            List<EmailAddress> emailAddress = new List<EmailAddress>();
            foreach (var item in cc)
            {
                emailAddress.Add(new EmailAddress(item));

            }
            var plainTextContent = "";
            var htmlContent = "<strong>Workflow</strong>";
            if (Convert.ToInt32(typeOfWorkflow) == 1)
            {
                subject = "Deactivation Workflow initiated";
            }
            if (Convert.ToInt32(typeOfWorkflow) == 2)
            {
                subject = "Activation Workflow initiated";

                string textBody = "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr><td><b>Surname</b></td> <td> <b> First Name </b> </td> <td><b>Surname</b></td> <td><b>Audiology Display Name</b></td> <td><b>Siemens e-mail ID</b></td> <td><b>Siemens Login</b></td> <td><b>Siemens GID</b></td> <td><b>Team</b></td><td><b>Role</b></td> <td><b>Gender</b></td> <td><b>Date of birth</b></td> <td><b>Place of birth</b></td> <td><b>Address - Street</b></td> <td><b>Address - City, Country</b></td> <td><b>Phone num</b></td> </tr> <tr><td></td></tr>";

                textBody += "</table>";

                htmlContent = "Dear Sevgi,<br>We have a new member joined our "+details.ActivatedEmployee.TeamName+" team. Could you please initiate the audiology account creation process ?<br> " + textBody + "Thanks & Regards,<br>Arun";

            }
            if (Convert.ToInt32(typeOfWorkflow) == 3)
            {
                subject = "Deactivation workflow";
                plainTextContent = "Today is " + details.EmployeeName + "'s last working day please check if you have approved the deactivation workflow";
            }
            if (Convert.ToInt32(typeOfWorkflow) == 4)
            {
                subject = "Deactivation workflow declined";
                plainTextContent = details.EmployeeName + " your account deactivation form has been declined";

            }
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            msg.AddCcs(emailAddress);
            if (details.ActivatedEmployee.ActivationWorkFlowPdfAttachment != null)
            {
                msg.AddAttachment(filename: details.FileName + ".pdf", Convert.ToBase64String(details.ActivatedEmployee.ActivationWorkFlowPdfAttachment));
            }
            _ = await client.SendEmailAsync(msg);
        }
    }
}
