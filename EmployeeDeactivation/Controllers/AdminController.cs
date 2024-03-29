﻿using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDeactivation.Controllers
{
    [Authorize("Admin&Manager")]
    public class AdminController : Controller 
    {
        private readonly IAdminDataOperation _adminDataOperation;
        public AdminController(IAdminDataOperation adminDataOperation)
        {
            _adminDataOperation = adminDataOperation;
        }

        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult AdminConfigurationPage()
        {
            return View();
        }

        public IActionResult AccountDeactivationDatePage()
        {
            return View(_adminDataOperation.DeactivationEmployeeData());
        }
        
        public IActionResult AccountActivationDetailsPage()
        {
            return View(_adminDataOperation.ActivationEmployeeData());
        }


        [HttpGet]
        [Route("Admin/SponsorDetails")]
        public JsonResult SponsorDetails()
        {
            return Json(_adminDataOperation.RetrieveSponsorDetails());
        }
        [HttpPost]
        [Route("Admin/SponsorDetailsAccordingToTeamName")]
        public JsonResult SponsorDetailsAccordingToGid(string teamName)
        {
            return Json(_adminDataOperation.RetrieveSponsorDetailsAccordingToTeamName(teamName));
        }

        [HttpPost]
        [Route("Admin/AddSponsorDetailsToDatabase")]
        public JsonResult AddSponsorDetailsToDatabase(Teams team)
        {

            return Json( _adminDataOperation.AddSponsorData(team));
        }

        [HttpPost]
        [Route("Admin/DeleteSponsorDetail")]
        public JsonResult DeleteSponsorDetails(string teamName)
        { 
            return Json(_adminDataOperation.DeleteSponsorData(teamName));
        }


    }
}