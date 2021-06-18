using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{

    public class PdfDataOperation : IPdfDataOperation
    {
        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly IManagerApprovalOperation _managerApprovalOperation;
        private readonly IAdminDataOperation _adminDataOperation;

        public PdfDataOperation(IEmployeeDataOperation employeeDataOperation, IManagerApprovalOperation managerApprovalOperation , IAdminDataOperation adminDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
            _managerApprovalOperation = managerApprovalOperation;
            _adminDataOperation = adminDataOperation;
        }

        public byte[] FillPdfForm(string GId)
        {
            var employeeData = _employeeDataOperation.RetrieveEmployeeDataBasedOnGid(GId);
            FileStream docStream = new FileStream("DeactivationFormPDF.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            (form.Fields[11] as PdfLoadedTextBoxField).Text = employeeData.FirstName;
            (form.Fields[10] as PdfLoadedTextBoxField).Text = employeeData.LastName;
            (form.Fields[13] as PdfLoadedTextBoxField).Text = employeeData.EmailID;
            (form.Fields[12] as PdfLoadedTextBoxField).Text = employeeData.GId;
            (form.Fields[4] as PdfLoadedTextBoxField).Text = employeeData.LastWorkingDate.ToString();
            string sponsorFullName = employeeData.SponsorName;
            string[] splitsponsorFullName = sponsorFullName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = splitsponsorFullName[0];
            (form.Fields[17] as PdfLoadedTextBoxField).Text = splitsponsorFullName[1];
            (form.Fields[18] as PdfLoadedTextBoxField).Text = employeeData.SponsorGId;
            (form.Fields[19] as PdfLoadedTextBoxField).Text = employeeData.SponsorDepartment;
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            byte[] bytes = stream.ToArray();
            return bytes;
        }

        public byte[] FillActivationPdfForm(string GId)
        {
            var activationEmployeeData = _employeeDataOperation.RetrieveActivationDataBasedOnGid(GId);
            FileStream docStream = new FileStream("Activation.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            PdfLoadedComboBoxField loadedListBox = form.Fields[8] as PdfLoadedComboBoxField;
            for (int i = 0; i < loadedListBox.Values.Count; i++)
            {
                if (loadedListBox.Values[i].Value == activationEmployeeData.Gender)
                {
                    loadedListBox.SelectedValue = activationEmployeeData.Gender;
                }
            }
            (form.Fields[9] as PdfLoadedTextBoxField).Text = activationEmployeeData.DateOfBirth.ToString();
            (form.Fields[10] as PdfLoadedTextBoxField).Text = activationEmployeeData.LastName;
            (form.Fields[11] as PdfLoadedTextBoxField).Text = activationEmployeeData.FirstName;
            (form.Fields[12] as PdfLoadedTextBoxField).Text = activationEmployeeData.Address;
            (form.Fields[13] as PdfLoadedTextBoxField).Text = activationEmployeeData.PlaceOfBirth;
            string sponsorFullName = activationEmployeeData.SponsorName;
            string[] splitsponsorFullName = sponsorFullName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = splitsponsorFullName[0];
            (form.Fields[17] as PdfLoadedTextBoxField).Text = splitsponsorFullName[1];
            (form.Fields[18] as PdfLoadedTextBoxField).Text = activationEmployeeData.GId;
            (form.Fields[19] as PdfLoadedTextBoxField).Text = activationEmployeeData.SponsorDepartment;
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            byte[] bytes = stream.ToArray();
            return bytes;
        }

        public void SendPdfAsEmailAttachmentDeactivation(string memoryStream, string employeeName, string teamName)
        {
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            var c = bytes;
            MemoryStream stream = new MemoryStream(bytes);
            Attachment file = new Attachment(stream, "Deactivation workflow_" + employeeName + ".pdf", "application/pdf");
            SendEmail(reportingManagerEmailId,employeeName,false,false, true, file);
        }

        public void SendPdfAsEmailAttachmentActivation(string memoryStream, string employeeName, string teamName, string sponsorGID,string siemensGid)
        {
            var reportingManagerEmailId = _employeeDataOperation.GetReportingManagerEmailId(teamName);
            var sponsorEmailId = _employeeDataOperation.GetSponsorEmailId(sponsorGID);
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            _employeeDataOperation.savepdf(bytes, siemensGid);
            var c = bytes;
            MemoryStream stream = new MemoryStream(bytes);
            Attachment file = new Attachment(stream, "Activation workflow_" + employeeName + ".pdf", "application/pdf");
            SendEmail(reportingManagerEmailId, employeeName, false,false, true, file);
            SendEmail(sponsorEmailId, employeeName, false,false, true, file);
            SendEmail("1by16cs072@bmsit.in", employeeName,false,false, true, file);            

        }

        public void SendEmailDeclined(string gid, string employeeName)
        {
            var employeeManagerEmailId = _employeeDataOperation.GetEmployeeEmailId(gid);
            Attachment file = null;
            SendEmail(employeeManagerEmailId, employeeName, false,true, false, file);

        }


        public void SendReminderEmail()
        {       
            var employeeDetails = _managerApprovalOperation.GetAllPendingDeactivationWorkflows();
            foreach (var item in employeeDetails)
            {
                string iDate = item.EmployeeLastWorkingDate;
                DateTime date = Convert.ToDateTime(iDate);
                if (DateTime.Today == date || DateTime.Today > date)
                {
                    MemoryStream stream = new MemoryStream(item.DeactivationWorkFlowPdfAttachment);
                    Attachment file = new Attachment(stream, "Deactivation workflow_" + item.EmployeeName + ".pdf", "application/pdf");
                    SendEmail(item.ReportingManagerEmail, item.EmployeeName, true , false , false, file);
                }  
            }
            var approvedEmployeeDetails = _managerApprovalOperation.GetAllApprovedDeactivationWorkflows() ;
            var employeeData = _employeeDataOperation.SavedEmployeeDetails();            
            foreach (var item in approvedEmployeeDetails)
            {
                if (DateTime.Today.ToString() == item.EmployeeLastWorkingDate)
                {
                    foreach (var employee in employeeData)
                    {
                        if(employee.GId == item.EmployeeGId)
                        {
                            MemoryStream stream = new MemoryStream(item.DeactivationWorkFlowPdfAttachment);
                            Attachment file = new Attachment(stream, "Deactivation workflow_" + item.EmployeeName + ".pdf", "application/pdf");
                            SendEmail(employee.SponsorEmailID, item.EmployeeName, true,false,false, file);
                        }
                    }
                }
            }
        }


        private void SendEmail(string Email , string employeeName, bool isReminderEmail, bool isDeclinedEmail,bool isActivationDeactivationEmail, Attachment file )
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("dontreplydeactivationworkflow@gmail.com");
            message.Sender = new MailAddress("dontreplydeactivationworkflow@gmail.com");
            message.To.Add(Email);
            if (isDeclinedEmail)
            {
                message.Subject = "Deactivation workflow declined";
                message.Body = employeeName + " your account deactivation form has been declined";
                
            }
            if (isActivationDeactivationEmail)
            {
                message.Subject = "Workflow initiated";
                message.Attachments.Add(file);
            }
            if (isReminderEmail)
            {
                message.Subject = "Deactivation workflow";
                message.Body = "Today is " + employeeName+"'s last working day please check if you have approved the deactivation workflow";
                message.Attachments.Add(file);
            }
            
            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("dontreplydeactivationworkflow@gmail.com", "Siemens@Banglore98");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);

        }
    }
}
