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


        [HttpPost]
        [Route("Email/AddConfigurationToDatabase")]
        public JsonResult AddConfigurationToDatabase(string ActivationMail , string DeactivationMail, string ReminderMail, string DeclinedMail)
        {

            return Json(_emailOperation.AddMailConfigurationData(ActivationMail, DeactivationMail, ReminderMail, DeclinedMail));
        }

        //[HttpPost]
        //[Route("Email/AddMailContentsToDatabase")]
        //public JsonResult AddMailContentsToDatabase(MailContent mailContent)
        //{

        //    return Json(_emailOperation.AddMailContentData(mailContent));
        //}

        //[HttpGet]
        //[Route("Email/MailDetails")]
        //public JsonResult MailDetails()
        //{
        //    return Json(_emailOperation.RetrieveAllMailContent());
        //}

        //[HttpPost]
        //[Route("Email/AddTokenDetalsToDatabase")]
        //public JsonResult AddTokenDetalsToDatabase(Token token)
        //{

        //    return Json(_emailOperation.AddTokenData(token));
        //}

        //[HttpGet]
        //[Route("Email/TokenDetails")]
        //public JsonResult TokenDetails()
        //{
        //    return Json(_emailOperation.RetrieveAllToken());
        //}

        [HttpPost]
        [Route("Email/PdfAttachmentEmail")]
        public JsonResult PdfAttachmentEmail(byte[] pdfFileArray, string employeeName, string teamName, bool isActivationPDf , string siemensGID)
        {
           return Json(_emailOperation.SendPDfAsEmailAttachment(new EmailDetails() { ActivatedEmployee = new ActivationEmployeeDetails() 
           { ActivationWorkFlowPdfAttachment = pdfFileArray, TeamName = teamName, GId = siemensGID }, EmployeeName = employeeName}, isActivationPDf));
        }

        [HttpGet]
        [Route("Email/SendReminder")]
        public void SendReminder()
        {
            _emailOperation.SendReminderEmail();
        }

        [HttpPost]
        [Route("Email/DeclineEmail")]
        public void DeclineEmail(string gId, string employeeName)
        {
            _emailOperation.SendEmailDeclined(gId, employeeName);
        }
    }
}
