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
        private readonly IEmployeeDataOperation _employeeDataOperation;
        private readonly EmployeeDeactivationContext _context;

        public AdminDataOperation(IEmployeeDataOperation employeeDataOperation , EmployeeDeactivationContext context)
        {
            _employeeDataOperation = employeeDataOperation;
            _context = context;
        }

        public List<Teams> RetrieveSponsorDetails()
        {
            return _context.Teams.ToList();
        }

        public bool AddSponsorData(Teams team)
        {
            bool databaseUpdateStatus = false;
            //Teams sponsor = new Teams()
            //{
            //    TeamName = team.TeamName,
            //    SponsorFirstName = team.SponsorFirstName,
            //    SponsorLastName = team.SponsorLastName,
            //    SponsorGID = team.SponsorGID,
            //    SponsorEmailID = team.SponsorEmailID,
            //    Department = team.Department,
            //    ReportingManagerEmailID = team.ReportingManagerEmailID,

            //};
            var teamDetails = _context.Teams.ToList();
            foreach (var teams in teamDetails)
            {
                if (teams.SponsorGID == team.SponsorGID)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == team.SponsorGID));
                    _context.SaveChanges();
                }
            }
            _context.Add(team);
            databaseUpdateStatus = _context.SaveChanges() == 1;
            return databaseUpdateStatus;
        }

        public bool DeleteSponsorData(string gId)
        {
            var teamDetails = _context.Teams.ToList();
            foreach (var teams in teamDetails)
            {
                if (teams.SponsorGID == gId)
                {
                    _context.Remove(_context.Teams.Single(a => a.SponsorGID == gId));
                    return _context.SaveChanges() == 1;
                    
                }
            }
            return true;

        }

        public List<EmployeeDetails> DeactivationEmployeeData()
        {
            return _employeeDataOperation.RetrieveAllDeactivatedEmployees();
        }



        public List<EmployeeDetails> ActivationEmployeeData()
        {
            return _employeeDataOperation.RetrieveAllActivationWorkFlow();
        }

    }
}
