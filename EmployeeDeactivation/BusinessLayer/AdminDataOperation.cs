using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;


namespace EmployeeDeactivation.BusinessLayer
{
    public class AdminDataOperation : IAdminDataOperation
    {

        private readonly EmployeeDeactivationContext _context;
        public AdminDataOperation(EmployeeDeactivationContext context)
        {
            _context = context;
        }


        public List<Teams> RetrieveSponsorDetails()
        {
            List<Teams> teamDetails = new List<Teams>();
            var details = _context.Teams.ToList();
            foreach (var item in details)
            {
                teamDetails.Add(new Teams
                {
                    SponsorGID = item.SponsorGID,
                    TeamName = item.TeamName,
                    SponsorFirstName = item.SponsorFirstName,
                    SponsorLastName = item.SponsorLastName,
                    SponsorEmailID = item.SponsorEmailID,
                    Department = item.Department,
                    ReportingManagerEmailID = item.ReportingManagerEmailID
                });
            }
            return teamDetails;
        }

        public async Task<bool> AddSponsorData(string teamName, string sponsorFirstName, string sponsorLastName, string sponsorGid, string sponsorEmail, string sponsorDepartment, string reportingManagerEmail)
        //review change make parameters as class
        {
            bool databaseUpdateStatus=false;
            Teams sponsor = new Teams()
            {
                TeamName = teamName,
                SponsorFirstName = sponsorFirstName,
                SponsorLastName = sponsorLastName,
                SponsorGID = sponsorGid,
                SponsorEmailID = sponsorEmail,
                Department = sponsorDepartment,
                ReportingManagerEmailID = reportingManagerEmail,
                
            };
            var check = _context.Teams.ToList();
            foreach (var i in check)
            {
                if (i.SponsorGID == sponsorGid)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == sponsorGid));
                    _context.SaveChanges();
                   
                }
            }
            _context.Add(sponsor);
            databaseUpdateStatus = _context.SaveChanges() == 1 ? true : false;

            return databaseUpdateStatus;
        }
        public async Task<bool> DeleteSponsorData(string gId)
        //review change make parameters as class
        {
          
            var check = _context.Teams.ToList();
            foreach (var i in check)
            {
                if (i.SponsorGID == gId)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == gId));
                    _context.SaveChanges();
                }
            }

            var databaseUpdateStatus = await _context.SaveChangesAsync() == 1 ? true : false;
            return databaseUpdateStatus;
        }

        public List<EmployeeDetails> RetrieveEmployeeDetails()
        {
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            var details = _context.DeactivationWorkflow.ToList();
            var sortedDetails = from p in details orderby p.LastWorkingDate select p;
            foreach (var item in sortedDetails)
            {
               
                employeeDetails.Add(new EmployeeDetails
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    EmailID = item.EmailID,
                    GId = item.GId,
                    LastWorkingDate = item.LastWorkingDate,
                    TeamName = item.TeamName,
                    SponsorName = item.SponsorName,
                    SponsorEmailID = item.SponsorEmailID,
                    SponsorDepartment = item.SponsorDepartment,
                });
            }
            return employeeDetails;
        }

        public List<EmployeeDetails> DeactivationEmployeeData()
        {
            var deactivationEmployeeData = (from deactivationEmployee in this._context.DeactivationWorkflow.Take(30000)select deactivationEmployee).ToList();
            return deactivationEmployeeData;
        }

        public List<EmployeeDetails> ActivationEmployeeData()
        {
            List<EmployeeDetails> activationEmployeeData = (from activationEmployee in this._context.ActivationWorkflow.Take(30000) select activationEmployee).ToList();
            return activationEmployeeData;
        }

    }   
}
