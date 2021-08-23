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
        public Teams RetrieveSponsorDetailsAccordingToTeamName(string teamName)
        {
            var allSponsors = _context.Teams.ToList();
            foreach (var item in allSponsors)
            {
                if(item.TeamName.ToLower()==teamName.ToLower())
                {
                    return item;
                }
            }
            return new Teams();           
        }

        public bool AddSponsorData(Teams team)
        {
        try{
            bool databaseUpdateStatus = false;
            var teamDetails = _context.Teams.ToList();
            foreach (var teams in teamDetails)
            {
                if (teams.TeamName.ToLower() == team.TeamName.ToLower())
                {
                    _context.Remove(_context.Teams.Single(a => a.TeamName.ToLower() == team.TeamName.ToLower()));
                    _context.SaveChanges();
                }
            }
            _context.Add(team);
           
                databaseUpdateStatus = _context.SaveChanges() == 1;
                return databaseUpdateStatus;

            }
        catch{
                return false;
            }     
        }

        public bool DeleteSponsorData(string teamName)
        {
            try
            {
                var teamDetails = _context.Teams.ToList();
                foreach (var teams in teamDetails)
                {
                    if (teams.TeamName.ToLower() == teamName.ToLower())
                    {
                        _context.Remove(_context.Teams.Single(a => a.TeamName.ToLower() == teamName.ToLower()));
                        return _context.SaveChanges() == 1;

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

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
