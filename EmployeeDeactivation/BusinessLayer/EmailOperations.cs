using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmailOperations:IEmailOperation
    {

        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly IManagerApprovalOperation _managerApprovalOperation;
        private readonly EmployeeDeactivationContext _context;


        public EmailOperations(IEmployeeDataOperation employeeDataOperation, IManagerApprovalOperation managerApprovalOperation, EmployeeDeactivationContext context)
        {
            _employeeDataOperation = employeeDataOperation;
            _managerApprovalOperation = managerApprovalOperation;
            _context = context;
        }
        #region MailConfiguration
        public string RetrieveSpecificConfiguration(string key)
        {
            var configurations = RetrieveAllMailContent();
            foreach (var item in configurations)
            {
                if (item.TokenName == key)
                {
                    return item.TokenValue;
                }
            }
            return string.Empty;
        }
        public List<Tokens> RetrieveAllMailContent()
        {
            return _context.Tokens.ToList();
        }
        public bool AddMailConfigurationData(string ActivationMail, string DeactivationMail,string  ReminderMail, string DeclinedMail,string SendGrid, string EmailTimer)
        {
            try
            {
                bool databaseUpdateStatus = false;
                var ConfigDetails = _context.Tokens.ToList();
                foreach (var Config in ConfigDetails)
                {
                    
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "ActivationMail"));
                    Tokens tokens = new Tokens();
                    tokens.TokenName = "ActivationMail";
                    tokens.TokenValue = ActivationMail;
                    _context.Add(tokens);
                    _context.SaveChanges();
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "DeactivationMail"));
                    tokens.TokenName = "DeactivationMail";
                    tokens.TokenValue = DeactivationMail;
                    _context.Add(tokens);
                    _context.SaveChanges();
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "ReminderMail"));
                    tokens.TokenName = "ReminderMail";
                    tokens.TokenValue = ReminderMail;
                    _context.Add(tokens);
                    _context.SaveChanges();
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "DeclinedMail"));
                    tokens.TokenName = "DeclinedMail";
                    tokens.TokenValue = DeclinedMail;
                    _context.Add(tokens);
                    _context.SaveChanges();
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "SendGrid"));
                    tokens.TokenName = "SendGrid";
                    tokens.TokenValue = SendGrid;
                    _context.Add(tokens);
                    _context.SaveChanges();
                    _context.Remove(_context.Tokens.Single(a => a.TokenName == "EmailTimer"));
                    tokens.TokenName = "EmailTimer";
                    tokens.TokenValue = EmailTimer;
                    _context.Add(tokens);
                    _context.SaveChanges();

                }
                databaseUpdateStatus = true;
                return databaseUpdateStatus;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion
        public bool SendPDfAsEmailAttachment(EmailDetails details,bool isActivationPdf)
        {
            var fileName = isActivationPdf ? "Activation workflow_" : "Deactivation workflow_";
            var emailDetails = _employeeDataOperation.GetReportingEmailIds(details.ActivatedEmployee.TeamName);
            if(emailDetails == null)
            {
                return false;
            }

            if (!isActivationPdf)
            {
                details.FromEmailId = emailDetails[3];
                details.ToEmailId = emailDetails[0];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
                _ = SendEmailAsync(details, TypeOfWorkflow.Deactivation);

            }
            if (isActivationPdf)
            {
                details.FromEmailId = emailDetails[0];
                details.ToEmailId = emailDetails[1];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
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
                            _ = SendEmailAsync(details , TypeOfWorkflow.ReminderEmail);
                        }
                    }
                }
                var twoDaysAgo=Convert.ToDateTime(approvedEmployee.EmployeeLastWorkingDate).AddDays(-2);
                if (DateTime.Today.ToString() == twoDaysAgo.ToString())
                {
                    foreach (var employee in employeeData)
                    {
                        if (employee.GId.ToLower() == approvedEmployee.EmployeeGId.ToLower())
                        {
                            EmailDetails details = new EmailDetails()
                            {
                                FromEmailId = employee.FromEmailId,
                                ToEmailId = employee.ReportingManagerEmail,
                                EmployeeName = approvedEmployee.EmployeeName
                            };
                            _ = SendEmailAsync(details, TypeOfWorkflow.ReminderEmail);
                        }
                    }
                }
            }
        }
       
        private async Task SendEmailAsync(EmailDetails details, Enum typeOfWorkflow)
        {
            var apiKey = RetrieveSpecificConfiguration("SendGrid");
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
                htmlContent = RetrieveSpecificConfiguration("DeactivationMail");
            }
            if (Convert.ToInt32(typeOfWorkflow) == 2)
            {
                var activationEmployeeDetails = _employeeDataOperation.RetrieveActivationDataBasedOnGid(details.ActivatedEmployee.GId);
                subject = "Activation Workflow initiated";
                string textBody = "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr> <th><b>Surname</b></th> <th><b>First Name</b></th> <th><b>Audiology Display Name</b></th> <th><b>Siemens e-mail ID</b></th> <th><b>Siemens Login</b></th> <th><b>Siemens GID</b></th> <th><b>Team</b></th> <th><b>Role</b></th> <th><b>Gender</b></th> <th><b>Date of birth</b></th> <th><b>Place of birth</b></th> <th><b>Address - Street</b></th> <th><b>Address - City, Country</b></th> <th><b>Phone num</b></th> <th><b>Nationality</b></th> </tr> <tr><td>" + activationEmployeeDetails.LastName + "</td> <td>" + activationEmployeeDetails.FirstName + "</td> <td>" + details.EmployeeName + "</td> <td>" + activationEmployeeDetails.EmailID + "</td> <td>" + details.ActivatedEmployee.GId + "</td> <td>" + activationEmployeeDetails.GId + "</td> <td>" + details.ActivatedEmployee.TeamName + "</td> <td>" + activationEmployeeDetails.Role + "</td>  <td>" + activationEmployeeDetails.Gender + "</td> <td>" + activationEmployeeDetails.DateOfBirth + "</td> <td>" + activationEmployeeDetails.PlaceOfBirth + "</td> <td>" + activationEmployeeDetails.Address + "</td> <td>" + activationEmployeeDetails.Address + "</td> <td>" + activationEmployeeDetails.PhoneNo + "</td> <td>" + activationEmployeeDetails.Nationality + "</td></tr>";
                textBody += "</table>";
                htmlContent = plainTextContent = RetrieveSpecificConfiguration("ActivationMail");
                if (htmlContent.Contains("+TeamName+"))
                {
                    htmlContent=htmlContent.Replace("+TeamName+", details.ActivatedEmployee.TeamName);
                }
                if (htmlContent.Contains("+textBody+"))
                {
                    htmlContent = htmlContent.Replace("+textBody+", textBody);
                }
            }
            if (Convert.ToInt32(typeOfWorkflow) == 3)
            {
                subject = "Deactivation workflow";
                plainTextContent = RetrieveSpecificConfiguration("ReminderMail");
                if (plainTextContent.Contains("+EmployeeName+"))
                {
                    plainTextContent= plainTextContent.Replace("+EmployeeName+", details.EmployeeName);
                }
            }
           
            if (Convert.ToInt32(typeOfWorkflow) == 4)
            {
                subject = "Deactivation workflow declined";
                plainTextContent = RetrieveSpecificConfiguration("DeclinedMail");
                if (plainTextContent.Contains("+EmployeeName+"))
                {
                    plainTextContent=plainTextContent.Replace("+EmployeeName+", details.EmployeeName);
                }
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
