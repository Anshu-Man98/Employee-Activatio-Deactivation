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

        public PdfDataOperation(IEmployeeDataOperation employeeDataOperation)
        {
            _employeeDataOperation = employeeDataOperation;
        }

        public byte[] FillDeactivationPdfForm(string gId)
        {
            var employeeData = _employeeDataOperation.RetrieveDeactivatedEmployeeDataBasedOnGid(gId);
            FileStream docStream = new FileStream("DeactivationFormPDF.pdf", FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            (form.Fields[11] as PdfLoadedTextBoxField).Text = employeeData.FirstName ?? "";
            (form.Fields[10] as PdfLoadedTextBoxField).Text = employeeData.LastName ?? "";
            (form.Fields[13] as PdfLoadedTextBoxField).Text = employeeData.EmailID ?? "";
            (form.Fields[12] as PdfLoadedTextBoxField).Text = employeeData.GId ?? "";
            (form.Fields[4] as PdfLoadedTextBoxField).Text = employeeData.LastWorkingDate.ToString() ?? "";
            string[] sponsorFullName = employeeData.SponsorName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = sponsorFullName[0] ?? "";
            (form.Fields[17] as PdfLoadedTextBoxField).Text = sponsorFullName[1] ?? "";
            (form.Fields[18] as PdfLoadedTextBoxField).Text = employeeData.SponsorGId ?? "";
            (form.Fields[19] as PdfLoadedTextBoxField).Text = employeeData.SponsorDepartment ?? "";
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            return stream.ToArray();
        }

        public byte[] FillActivationPdfForm(string gId)
        {
            var activationEmployeeData = _employeeDataOperation.RetrieveActivationDataBasedOnGid(gId);
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
            (form.Fields[9] as PdfLoadedTextBoxField).Text = activationEmployeeData.DateOfBirth.ToString()??"";
            (form.Fields[10] as PdfLoadedTextBoxField).Text = activationEmployeeData.LastName ?? "";
            (form.Fields[11] as PdfLoadedTextBoxField).Text = activationEmployeeData.FirstName ?? "";
            (form.Fields[12] as PdfLoadedTextBoxField).Text = activationEmployeeData.Address ?? "";
            (form.Fields[13] as PdfLoadedTextBoxField).Text = activationEmployeeData.PlaceOfBirth ?? "";
            string[] sponsorFullName = activationEmployeeData.SponsorName.Split(' ');
            (form.Fields[16] as PdfLoadedTextBoxField).Text = sponsorFullName[0] ?? "";
            (form.Fields[17] as PdfLoadedTextBoxField).Text = sponsorFullName[1] ?? "";
            (form.Fields[19] as PdfLoadedTextBoxField).Text = activationEmployeeData.SponsorDepartment ?? "";
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            return stream.ToArray();
        }
        
    }
}
