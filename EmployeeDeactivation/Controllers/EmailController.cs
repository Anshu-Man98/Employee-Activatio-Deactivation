﻿using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{

    public class EmailController : Controller
    {
        private readonly IEmailOperation _emailOperation;

        public EmailController(IEmailOperation emailOperation)
        {
            _emailOperation = emailOperation;
        }

        [HttpGet]
        [Route("Email/GetConfigurationDetails")]
        public JsonResult GetConfigurationDetails()
        {
            return Json(_emailOperation.RetrieveAllMailContent());
        }

        [HttpPost]
        [Route("Email/AddConfigurationToDatabase")]
        public JsonResult AddConfigurationToDatabase(string SendGrid, string EmailTimer, string ToEmailAUDomain, string ContactNameAUDomain, string CCEmailAUDomain, string WelcomeEmailSender)
        {
            
            return Json(_emailOperation.AddMailConfigurationData(SendGrid, EmailTimer, ToEmailAUDomain, ContactNameAUDomain, CCEmailAUDomain, WelcomeEmailSender));
        }

        [HttpPost]
        [Route("Email/AddMailContentToDatabase")]
        public JsonResult AddMailContentToDatabase(string ActivationMailInitiated, string DeactivationMailInitiated,  string DeactivationMailLastWorkingDayToSponsor, string DeactivationWorkflowDaysBeforeRemainder, string DeactivationWorkflowToEmployeeRemainder ,string ActivationWorkFlowRemainderToManager, string ActivationWorkFlowRemainderToEmployee, string EmailToAssignAUdomainWBT /*, string DeclinedMail, string DeactivationMailLastWorkingDayToManager*/)
        {

            return Json(_emailOperation.AddMailContentData(ActivationMailInitiated, DeactivationMailInitiated, DeactivationMailLastWorkingDayToSponsor , DeactivationWorkflowDaysBeforeRemainder, DeactivationWorkflowToEmployeeRemainder, ActivationWorkFlowRemainderToManager, ActivationWorkFlowRemainderToEmployee, EmailToAssignAUdomainWBT /*, DeactivationMailLastWorkingDayToManager, DeclinedMail*/));
        }

        [HttpPost]
        [Route("Email/PdfAttachmentEmail")]
        public JsonResult PdfAttachmentEmail(byte[] pdfFileArray, string employeeName, string teamName, bool isActivationPDf , string siemensGID,string employeeEmail ,string managerEmail)
        {
           return Json(_emailOperation.SendPDfAsEmailAttachment(new EmailDetails() { ActivatedEmployee = new ActivationEmployeeDetails()
           { ActivationWorkFlowPdfAttachment = pdfFileArray, TeamName = teamName, GId = siemensGID },
               EmployeeName = employeeName
           }, isActivationPDf,employeeEmail));
        }

        [HttpPost]
        [Route("Email/SendWelcomeEmail")]
        public JsonResult SendWelcomeEmail(WelcomeCard welcomeCardInfo)
        {
            return Json(_emailOperation.SendWelcomeEmail(welcomeCardInfo));
        }

        [HttpGet]
        [Route("Email/SendReminder")]
        public void SendReminder() => _emailOperation.SendReminderEmail();

        //[HttpPost]
        //[Route("Email/DeclineEmail")]
        //public void DeclineEmail(string gId, string employeeName)
        //{
        //    _emailOperation.SendEmailDeclined(gId, employeeName);
        //}
    }
}
