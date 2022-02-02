using System.Collections.Generic;
using System.Linq;
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
            var teamDetails = _context.Teams.Find(teamName);
            if (teamDetails == null)
            {
                return null;
            }
            else
            {
                return teamDetails;
            }
            //var allSponsors = _context.Teams.ToList();
            //foreach (var item in allSponsors)
            //{
            //    if(item.TeamName.ToLower()==teamName.ToLower())
            //    {
            //        return item;
            //    }
            //}
            //return new Teams();           
        }

        public bool AddSponsorData(Teams team)
        {
        try{
                if(team == null)
                {
                    return false;
                }
                bool databaseUpdateStatus = false;
                var teamDetails = _context.Teams.Find(team.TeamName);
                if (teamDetails == null)
                {
                    _context.Teams.Add(team);
                }
                else
                {
                    _context.Remove(teamDetails);
                    _context.Teams.Add(team);

                }
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
                var sponsorData = _context.Teams.Find(teamName);
                if(sponsorData == null)
                {
                    return false;
                }
                _context.Remove(sponsorData);
                _context.SaveChanges();
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
 