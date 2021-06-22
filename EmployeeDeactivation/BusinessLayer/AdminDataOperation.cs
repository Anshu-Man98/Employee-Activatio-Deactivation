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
            return _context.Teams.ToList();
        }

        public bool AddSponsorData(Teams team)
        {
            bool databaseUpdateStatus = false;
            Teams sponsor = new Teams()
            {
                TeamName = team.TeamName,
                SponsorFirstName = team.SponsorFirstName,
                SponsorLastName = team.SponsorLastName,
                SponsorGID = team.SponsorGID,
                SponsorEmailID = team.SponsorEmailID,
                Department = team.Department,
                ReportingManagerEmailID = team.ReportingManagerEmailID,

            };
            var teamDetails = _context.Teams.ToList();
            foreach (var teams in teamDetails)
            {
                if (teams.SponsorGID == team.SponsorGID)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == team.SponsorGID));
                    _context.SaveChanges();
                }
            }
            _context.Add(sponsor);
            databaseUpdateStatus = _context.SaveChanges() == 1;
            return databaseUpdateStatus;
        }
        public async Task<bool> DeleteSponsorData(string gId)
        {
            var teamDetails = _context.Teams.ToList();
            foreach (var teams in teamDetails)
            {
                if (teams.SponsorGID == gId)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == gId));
                    _context.SaveChanges();
                }
            }
            var databaseUpdateStatus = await _context.SaveChangesAsync() == 1;
            return databaseUpdateStatus;
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
