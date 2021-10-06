using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmailOperations : IEmailOperation
    {

        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly IManagerApprovalOperation _managerApprovalOperation;
        private readonly EmployeeDeactivationContext _context;
        private readonly SendGridClient sendGridClientApiKey;
        private readonly List<Tokens> configurations;

        public EmailOperations(IEmployeeDataOperation employeeDataOperation, IManagerApprovalOperation managerApprovalOperation, EmployeeDeactivationContext context)
        {
            _employeeDataOperation = employeeDataOperation;
            _managerApprovalOperation = managerApprovalOperation;
            _context = context;
            configurations = RetrieveAllMailContent();
            sendGridClientApiKey = new SendGridClient(RetrieveSpecificConfiguration("SendGrid"));
        }
        #region MailConfiguration
        public string RetrieveSpecificConfiguration(string key)
        {
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
            return (_context.Tokens.ToList());
        }

        public bool AddMailConfigurationData(string SendGrid, string EmailTimer)
        {
            try
            {
                bool databaseUpdateStatus = false;
                var ConfigDetails = _context.Tokens.ToList();
                foreach (var Config in ConfigDetails)
                {

                    if (Config.TokenName == "SendGrid")
                    {
                        Config.TokenValue = SendGrid;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "EmailTimer")
                    {
                        Config.TokenValue = EmailTimer;
                        _context.SaveChanges();
                    }

                }
                databaseUpdateStatus = true;
                return databaseUpdateStatus;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddMailContentData(string ActivationMailInitiated, string DeactivationMailInitiated, string DeactivationMailLastWorkingDayToSponsor, string DeactivationWorkflowDaysBeforeRemainder, string DeactivationWorkflowToEmployeeRemainder, string ActivationWorkFlowRemainderToManager, string ActivationWorkFlowRemainderToEmployee, string EmailToAssignAUdomainWBT /*, string DeclinedMail, string DeactivationMailLastWorkingDayToManager*/)
        {
            try
            {
                bool databaseUpdateStatus = false;
                var ConfigDetails = _context.Tokens.ToList();
                foreach (var Config in ConfigDetails)
                {

                    if (Config.TokenName == "ActivationMailInitiated")
                    {
                        Config.TokenValue = ActivationMailInitiated;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "DeactivationMailInitiated")
                    {
                        Config.TokenValue = DeactivationMailInitiated;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "DeactivationMailLastWorkingDayToSponsor")
                    {
                        Config.TokenValue = DeactivationMailLastWorkingDayToSponsor;
                        _context.SaveChanges();
                    }

                    if (Config.TokenName == "DeactivationWorkflowToEmployeeRemainder")
                    {
                        Config.TokenValue = DeactivationWorkflowToEmployeeRemainder;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "DeactivationWorkflowDaysBeforeRemainder")
                    {
                        Config.TokenValue = DeactivationWorkflowDaysBeforeRemainder;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "ActivationWorkFlowRemainderToManager")
                    {
                        Config.TokenValue = ActivationWorkFlowRemainderToManager;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "ActivationWorkFlowRemainderToEmployee")
                    {
                        Config.TokenValue = ActivationWorkFlowRemainderToEmployee;
                        _context.SaveChanges();
                    }
                    if (Config.TokenName == "EmailToAssignAUdomainWBT")
                    {
                        Config.TokenValue = EmailToAssignAUdomainWBT;
                        _context.SaveChanges();
                    }
                    //if (Config.TokenName == "DeactivationMailLastWorkingDayToManager")
                    //{
                    //    Config.TokenValue = DeactivationMailLastWorkingDayToManager;
                    //    _context.SaveChanges();
                    //}
                    //if (Config.TokenName == "DeclinedMail")
                    //{
                    //    Config.TokenValue = DeclinedMail;
                    //    _context.SaveChanges();
                    //}

                }
                databaseUpdateStatus = true;
                return databaseUpdateStatus;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        public bool SendPDfAsEmailAttachment(EmailDetails details, bool isActivationPdf)
        {
            var fileName = isActivationPdf ? "Activation workflow_" : "Deactivation workflow_";
            var emailDetails = _employeeDataOperation.GetReportingEmailIds(details.ActivatedEmployee.TeamName);
            if (emailDetails == null)
            {
                return false;
            }
            if (!isActivationPdf)
            {
                details.FromEmailId = emailDetails[3];
                details.ToEmailId = emailDetails[0];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
                _ = SendEmailAsync(details, TypeOfWorkflow.DeactivationWorkFlowInitiated,null,null);

            }
            if (isActivationPdf)
            {
                details.FromEmailId = emailDetails[0];
                details.ToEmailId = "1by16cs072@bmsit.in";
                details.CcEmailId ="anshumansunil98@gmail.com";
                details.FileName = null;
                _ = SendEmailAsync(details, TypeOfWorkflow.EmailToVivek,null,null);
                details.ToEmailId = emailDetails[1];
                details.CcEmailId = emailDetails[2];
                details.FileName = fileName + details.EmployeeName;
                _ = SendEmailAsync(details, TypeOfWorkflow.Activation,null,null);
            }
            return true;
        }
        //public async Task SendEmailDeclined(string gId, string employeeName)
        //{
        //    var employeeDetails = _employeeDataOperation.GetDeactivatedEmployeeDetails(gId);
        //    var reportingManagerEmailId = _employeeDataOperation.GetReportingEmailIds(employeeDetails[1])[3];
        //    byte[] byte_array = null;
        //    EmailDetails details = new EmailDetails()
        //    {
        //        FromEmailId = reportingManagerEmailId,
        //        ToEmailId = employeeDetails[0],
        //        CcEmailId = "",
        //        EmployeeName = employeeName,
        //        ActivatedEmployee = new ActivationEmployeeDetails()
        //        {
        //            ActivationWorkFlowPdfAttachment = byte_array,
        //            TeamName = String.Empty
        //        },
        //        FileName = String.Empty
        //    };
        //    await SendEmailAsync(details, TypeOfWorkflow.DeclinedEmail);
        //}


        public async Task SendReminderEmail()
        {
            //var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            var approvedEmployeeDetails = _managerApprovalOperation.GetAllApprovedDeactivationWorkflows();
            var employeeData = _employeeDataOperation.RetrieveAllDeactivatedEmployees();
            var activationEmployeeData = _employeeDataOperation.RetrieveAllActivationWorkFlow();
            var deactivationStatusEmployeeDetails = _managerApprovalOperation.RetrieveDeactivationTasksBasedOnDate();
            var activationStatusEmployeeDetails = _managerApprovalOperation.RetrieveActivationTasksBasedOnDate();
            var deactTask = _managerApprovalOperation.RetrieveDeactivationTasks();
            //await SendMailToManagerOnUnapprovedDeactivationWorkflowsOnLastWorkingDay(employeeDetails);
            //await SendDeactivationWorkFlowMailToSponsorOnLastWorkingDay(approvedEmployeeDetails);
            //await SendReminderMailForDeactivationTask(deactivationStatusEmployeeDetails);
            await SendReminderMailForActivationTask(activationStatusEmployeeDetails);
        }

        //private async Task SendMailToManagerOnUnapprovedDeactivationWorkflowsOnLastWorkingDay(List<ManagerApprovalStatus> employeeDetails)
        //{
        //    foreach (var employee in employeeDetails)
        //    {
        //        DateTime date = Convert.ToDateTime(employee.EmployeeLastWorkingDate.ToString());
        //        if (DateTime.Today == date || DateTime.Today > date)
        //        { 
        //            if(employee.WorkFlowStatus == "pending")

        //        {
        //            var emailDetails = _employeeDataOperation.GetReportingEmailIds(employee.EmployeeTeamName);
        //            EmailDetails details = new EmailDetails()
        //            {
        //                FromEmailId = emailDetails[0],
        //                ToEmailId = employee.ReportingManagerEmail,
        //                CcEmailId = "",
        //                EmployeeName = employee.EmployeeName,
        //                ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = employee.DeactivationWorkFlowPdfAttachment, TeamName = String.Empty },
        //                FileName = "DeactivationWorkflow_" + employee.EmployeeName
        //            };
        //            await SendEmailAsync(details, TypeOfWorkflow.DeactivationWorkFlowLastWorkingDay);
        //        }
        //        }
        //    }
        //}

        private async Task SendDeactivationWorkFlowMailToSponsorOnLastWorkingDay(List<ManagerApprovalStatus> approvedEmployeeDetails)
        {
            var empdata = _employeeDataOperation.RetrieveDeactivationWorkFlowBaseonDate(DateTime.Today.ToString());
            foreach (var DatedEmployee in empdata)
            {

                foreach (var approvedEmployee in approvedEmployeeDetails)
                {
                    if (DateTime.Today.ToString() == Convert.ToDateTime(approvedEmployee.EmployeeLastWorkingDate).ToString())
                    {

                        if (DatedEmployee.GId == approvedEmployee.EmployeeGId)
                        {
                            EmailDetails details = new EmailDetails()

                            {
                                FromEmailId = DatedEmployee.FromEmailId,
                                ToEmailId = DatedEmployee.SponsorEmailID,
                                CcEmailId = DatedEmployee.CcEmailId,
                                EmployeeName = approvedEmployee.EmployeeName,
                                ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = approvedEmployee.DeactivationWorkFlowPdfAttachment, TeamName = String.Empty },
                                FileName = "DeactivationWorkflow_" + approvedEmployee.EmployeeName
                            };
                            await SendEmailAsync(details, TypeOfWorkflow.DeactivationWorkFlowLastWorkingDay,null,null);
                        }
                    }
                }
            }
        }

        private async Task SendReminderMailForDeactivationTask(List<DeactivationStatus> deactivationStatusEmployeeDetails)
        {
            try
            {

                foreach (var Employees in deactivationStatusEmployeeDetails)
                {

                                EmailDetails details = new EmailDetails()
                                {
                                    FromEmailId = Employees.FromEmail,
                                    ToEmailId = Employees.ReportingManagerEmail,
                                    EmployeeName = Employees.EmployeeName,
                                    EmployeeId = Employees.EmployeeId,
                                    ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = null, TeamName = String.Empty },
                                };
                                DeactivationStatus deactivationTask = new DeactivationStatus()
                                {
                                    TimesheetApproval = Employees.TimesheetApproval,
                                    EmployeeRemovedFromDLEmailList = Employees.EmployeeRemovedFromDLEmailList,
                                    HardwaresCollected = Employees.HardwaresCollected,
                                    RaisedWindowsDeactivationRequestNexus = Employees.RaisedWindowsDeactivationRequestNexus,
                                 };
                                await SendEmailAsync(details, TypeOfWorkflow.DeactivationWorkFlowReminderManagerTwoDaysBeforeLastWorkingDay, deactivationTask,null);
                                details.ToEmailId = Employees.EmployeeEmail;
                                details.CcEmailId = Employees.ReportingManagerEmail;
                                await SendEmailAsync(details, TypeOfWorkflow.DeactivationWorkFlowReminderEmployee, null,null);
                }
            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorDeactivationTaskRemainder.txt";


                if (File.Exists(fileName))

                {

                    File.Delete(fileName);

                }

                // Create a new file     

                using (FileStream fs = File.Create(fileName))

                {

                    // Add some text to file    

                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");

                    fs.Write(title, 0, title.Length);
                    byte[] text = new UTF8Encoding(true).GetBytes("ERROR Retrive from ------------------> " + e.StackTrace);

                    fs.Write(text);
                    

                }
            }

        }

        private async Task SendReminderMailForActivationTask(List<ActivationStatus> activationStatusEmployeeDetails)
        {
            try
            {
                
                foreach (var Employees in activationStatusEmployeeDetails)
                {
                        EmailDetails details = new EmailDetails()
                        {
                            FromEmailId = Employees.FromEmail,
                            ToEmailId = Employees.ReportingManagerEmail,
                            EmployeeName = Employees.EmployeeName,
                            EmployeeId = Employees.EmployeeId,
                            ActivatedEmployee = new ActivationEmployeeDetails() { ActivationWorkFlowPdfAttachment = null, TeamName = String.Empty },
                        };
                    ActivationStatus activationTask = new ActivationStatus()
                    {
                        UpdateConsolidateIoTResourcesList = Employees.UpdateConsolidateIoTResourcesList,
                        BirthdayListUpdated = Employees.BirthdayListUpdated,
                        EmailIdAddedToTeamDL = Employees.EmailIdAddedToTeamDL,
                        MemberAddedToTimesheet = Employees.MemberAddedToTimesheet,
                        InitiateHardwareShipmentRequest = Employees.InitiateHardwareShipmentRequest,
                        MemberDetailsUpdatedInEDMTTool = Employees.MemberDetailsUpdatedInEDMTTool,
                        InductionPlanAssignment = Employees.InductionPlanAssignment,
                        InductionTrainingRecordUpdated = Employees.InductionTrainingRecordUpdated,
                        VivekEcampusListUpdated = Employees.VivekEcampusListUpdated,
                    };
                    await SendEmailAsync(details, TypeOfWorkflow.ActivationWorkFlowRemainderToManager, null, activationTask);
                    details.ToEmailId = Employees.EmployeeEmail;
                    details.WfhAttachment = true;
                    await SendEmailAsync(details, TypeOfWorkflow.ActivationWorkFlowRemainderToEmployee, null,null);
                    
                }
            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorActivationTaskRemainder.txt";


                if (File.Exists(fileName))

                {

                    File.Delete(fileName);

                }

                // Create a new file     

                using (FileStream fs = File.Create(fileName))

                {

                    // Add some text to file    

                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");

                    fs.Write(title, 0, title.Length);
                    byte[] text = new UTF8Encoding(true).GetBytes("ERROR Retrive from ------------------> " + e.StackTrace);

                    fs.Write(text);


                }
            }
        }


        private async Task SendEmailAsync(EmailDetails details, Enum typeOfWorkflow, DeactivationStatus deactivationTask , ActivationStatus activationTask)
        {
            try
            {
                List<EmailAddress> emailAddress = new List<EmailAddress>();
                var from = new EmailAddress(details.FromEmailId, "");
                var subject = "";
                var to = new EmailAddress(details.ToEmailId, "");
                if (!string.IsNullOrEmpty(details.CcEmailId))
                {
                    foreach (var item in details.CcEmailId.Split(","))
                    {
                        emailAddress.Add(new EmailAddress(item));
                    }
                }
                var plainTextContent = "";
                var htmlContent = "<strong>Workflow</strong>";
                if (Convert.ToInt32(typeOfWorkflow) == 1)
                {
                    subject = "Deactivation Workflow Initiated";
                    htmlContent = RetrieveSpecificConfiguration("DeactivationMailInitiated");
                    if (RetrieveSpecificConfiguration("DeactivationMailInitiated").Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }
                }
                if (Convert.ToInt32(typeOfWorkflow) == 2)
                {
                    subject = "Deactivation Workflow Initiated";
                    htmlContent = RetrieveSpecificConfiguration("DeactivationMailLastWorkingDayToSponsor");
                    if (RetrieveSpecificConfiguration("DeactivationMailLastWorkingDayToSponsor").Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }
                }
                if (Convert.ToInt32(typeOfWorkflow) == 3)
                {
                    subject = "Deactivation workflow";
                    htmlContent = RetrieveSpecificConfiguration("DeactivationMailOnLastWorkingDay");
                    if (htmlContent.Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }
                }
                if (Convert.ToInt32(typeOfWorkflow) == 4)
                {
                    subject = "Deactivation workflow";
                    htmlContent = RetrieveSpecificConfiguration("DeactivationWorkflowDaysBeforeRemainder");

                            if (deactivationTask.TimesheetApproval.Trim() == "true")
                            {
                                htmlContent = htmlContent.Replace("<li>Ensure timesheet is approved for exiting employee</li>\n", string.Empty);
                            }
                            if (deactivationTask.RaisedWindowsDeactivationRequestNexus.Trim() == "true")
                            {
                                htmlContent = htmlContent.Replace("<li>Raise windows account deactivation request in nexus</li>\n", string.Empty);
                            }
                            if (deactivationTask.EmployeeRemovedFromDLEmailList.Trim() == "true")
                            {
                                htmlContent = htmlContent.Replace("<li>Remove employee from DL email List</li>\n", string.Empty);
                            }
                            if (deactivationTask.HardwaresCollected.Trim() == "true")
                            {
                                htmlContent = htmlContent.Replace("<li>Ensure all hardware are collected</li>\n", string.Empty);
                            }

                            if (htmlContent.Contains("+EmployeeName+"))
                            {
                                htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                            }

                }
                if (Convert.ToInt32(typeOfWorkflow) == 5)
                {
                    subject = "Deactivation workflow";
                    htmlContent = RetrieveSpecificConfiguration("DeactivationWorkflowToEmployeeRemainder");
                    if (htmlContent.Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }
                }
                if (Convert.ToInt32(typeOfWorkflow) == 6)
                {
                    var activationEmployeeDetails = _employeeDataOperation.RetrieveActivationDataBasedOnGid(details.ActivatedEmployee.GId);
                    subject = "Activation Workflow initiated";
                    string textBody = "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr> <th><b>Surname</b></th> <th><b>First Name</b></th> <th><b>Audiology Display Name</b></th> <th><b>Siemens e-mail ID</b></th> <th><b>Siemens Login</b></th> <th><b>Siemens GID</b></th> <th><b>Team</b></th> <th><b>Role</b></th> <th><b>Gender</b></th> <th><b>Date of birth</b></th> <th><b>Place of birth</b></th> <th><b>Address - Street</b></th> <th><b>Address - City, Country</b></th> <th><b>Phone num</b></th> <th><b>Nationality</b></th> </tr> <tr><td>" + activationEmployeeDetails.LastName + "</td> <td>" + activationEmployeeDetails.FirstName + "</td> <td>" + details.EmployeeName + "</td> <td>" + activationEmployeeDetails.EmailID + "</td> <td>" + details.ActivatedEmployee.GId + "</td> <td>" + activationEmployeeDetails.GId + "</td> <td>" + details.ActivatedEmployee.TeamName + "</td> <td>" + activationEmployeeDetails.Role + "</td>  <td>" + activationEmployeeDetails.Gender + "</td> <td>" + activationEmployeeDetails.DateOfBirth + "</td> <td>" + activationEmployeeDetails.PlaceOfBirth + "</td> <td>" + activationEmployeeDetails.Address + "</td> <td>" + activationEmployeeDetails.Address + "</td> <td>" + activationEmployeeDetails.PhoneNo + "</td> <td>" + activationEmployeeDetails.Nationality + "</td></tr>";
                    textBody += "</table>";
                    htmlContent = RetrieveSpecificConfiguration("ActivationMailInitiated");
                    if (htmlContent.Contains("+TeamName+"))
                    {
                        htmlContent = htmlContent.Replace("+TeamName+", details.ActivatedEmployee.TeamName);
                    }
                    if (htmlContent.Contains("+textBody+"))
                    {
                        htmlContent = htmlContent.Replace("+textBody+", textBody);
                    }
                }
                if (Convert.ToInt32(typeOfWorkflow) == 7)
                {
                    subject = "Deactivation workflow declined";
                    htmlContent = RetrieveSpecificConfiguration("DeclinedMail");
                    if (htmlContent.Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }
                }

                if (Convert.ToInt32(typeOfWorkflow) == 8)
                {
                    subject = "Activation workflow";
                    htmlContent = RetrieveSpecificConfiguration("ActivationWorkFlowRemainderToManager");

                    if (activationTask.UpdateConsolidateIoTResourcesList.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Update Consolidated IoT resources list <a href= 'https://teams.microsoft.com/l/file/085270FB-8157-4748-AB95-090E525B38AD?tenantId=38ae3bcd-9579-4fd4-adda-b42e1495d55a&fileType=xlsx&objectUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226%2FShared%20Documents%2FGeneral%2FAUDI%2FInventoryDatabase.xlsx&baseUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226&serviceName=teams&threadId=19:fJdJXvBmz2s9CtNVZF_O_2SOUsPQAKwfeffJB7ostRs1@thread.tacv2&groupId=694fe3d1-5757-46c4-9c30-57bceec52994'>Click here</a></li> ", string.Empty);
                    }
                    if (activationTask.BirthdayListUpdated.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Birthday list to be updated <a href= 'https://teams.microsoft.com/l/file/6ED5BFBB-F186-41E5-A1CA-BF405FB2111F?tenantId=38ae3bcd-9579-4fd4-adda-b42e1495d55a&fileType=docx&objectUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226%2FShared%20Documents%2FGeneral%2FAUDI%2FWSA%20AU-Inventory.docx&baseUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226&serviceName=teams&threadId=19:fJdJXvBmz2s9CtNVZF_O_2SOUsPQAKwfeffJB7ostRs1@thread.tacv2&groupId=694fe3d1-5757-46c4-9c30-57bceec52994'> Click here </a></li>", string.Empty);
                    }
                    if (activationTask.EmailIdAddedToTeamDL.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Add email id to project/ team distribution list</li>", string.Empty);
                    }
                    if (activationTask.MemberAddedToTimesheet.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Add member to Timesheet</li>", string.Empty);
                    }
                    if (activationTask.InitiateHardwareShipmentRequest.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Initiate hardware shipment request in DTS / ISE office admin form for machine and accessories or Initiate office visit request in DTS / ISE office admin form incase member travelling to office for machine pickup</li>", string.Empty);
                    }
                    if (activationTask.MemberDetailsUpdatedInEDMTTool.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Update member details in EDMT tool </li>", string.Empty);
                    }
                    if (activationTask.InductionPlanAssignment.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Induction plan assignment </li>", string.Empty);
                    }
                    if (activationTask.InductionTrainingRecordUpdated.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Update induction training record <a href = 'https://teams.microsoft.com/l/file/085270FB-8157-4748-AB95-090E525B38AD?tenantId=38ae3bcd-9579-4fd4-adda-b42e1495d55a&fileType=xlsx&objectUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226%2FShared%20Documents%2FGeneral%2FAUDI%2FInventoryDatabase.xlsx&baseUrl=https%3A%2F%2Fsiemensapc.sharepoint.com%2Fteams%2FAUDI226&serviceName=teams&threadId=19:fJdJXvBmz2s9CtNVZF_O_2SOUsPQAKwfeffJB7ostRs1@thread.tacv2&groupId=694fe3d1-5757-46c4-9c30-57bceec52994'> Click here </a></li>", string.Empty);
                    }
                    if (activationTask.VivekEcampusListUpdated.Trim() == "true")
                    {
                        htmlContent = htmlContent.Replace("<li> Update Vivek's ecampus list</li>", string.Empty);
                    }

                    if (htmlContent.Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }

                }

                if (Convert.ToInt32(typeOfWorkflow) == 9)
                {
                    subject = "Activation workflow";
                    htmlContent = RetrieveSpecificConfiguration("ActivationWorkFlowRemainderToEmployee");

                    if (htmlContent.Contains("+EmployeeName+"))
                    {
                        htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                    }

                }

                if (Convert.ToInt32(typeOfWorkflow) == 10)
                {
                    subject = "Activation workflow";
                    htmlContent = RetrieveSpecificConfiguration("EmailToAssignAUdomainWBT");
                    var allActivationWorktasks = _managerApprovalOperation.RetrieveActivationTasks();
                            if (htmlContent.Contains("+EmployeeName+"))
                            {
                                htmlContent = htmlContent.Replace("+EmployeeName+", details.EmployeeName);
                            }
                }


                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                if (emailAddress.Count > 0)
                {
                    msg.AddCcs(emailAddress);
                }
                if (details.ActivatedEmployee.ActivationWorkFlowPdfAttachment != null && details.FileName != null)
                {
                    msg.AddAttachment(filename: details.FileName + ".pdf", Convert.ToBase64String(details.ActivatedEmployee.ActivationWorkFlowPdfAttachment));
                }
                if (details.WfhAttachment == true)
                {
                    FileStream Reimbursement = new FileStream("ReimbursementBroadbandOrMobileData.msg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    msg.AddAttachment(filename: "ReimbursementBroadbandOrMobileData.msg", Convert.ToString(Reimbursement));
                    FileStream Wfh = new FileStream("EnablingWorkFromHomeFurther.msg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    msg.AddAttachment(filename: "EnablingWorkFromHomeFurther.msg", Convert.ToString(Wfh));
                }
                
                _ = await sendGridClientApiKey.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}