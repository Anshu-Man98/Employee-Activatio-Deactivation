using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmployeeDataOperation : IEmployeeDataOperation
    {
        private readonly EmployeeDeactivationContext _context;
        public EmployeeDataOperation(EmployeeDeactivationContext context)
        {
            _context = context;
        }

    public string GetReportingManagerEmailId(string teamName)
        {
            var teamDetails = RetrieveAllSponsorDetails();
            foreach (var item in teamDetails)
            {
                if(item.TeamName == teamName)
                {
                    return item.ReportingManagerEmailID;
                }
            }
            return "";      
        }

        public string GetEmployeeEmailId(string gid)
        {
            var DeactivationDetails = SavedEmployeeDetails();
            foreach (var item in DeactivationDetails)
            {
                if (item.GId == gid)
                {
                    return item.EmailID;
                }
            }
            return "";
        }

        public string GetSponsorEmailId(string SponsorGid)
        {
            var teamDetails = RetrieveAllSponsorDetails();
            foreach (var item in teamDetails)
            {
                if (item.SponsorGID == SponsorGid)
                {
                    return item.SponsorEmailID;
                }
            }
            return "";
        }

        public  List<EmployeeDetails> SavedEmployeeDetails()
        {
            List<EmployeeDetails> userDetails = new List<EmployeeDetails>();
            var info =  _context.DeactivationWorkflow.ToList();
            foreach (var item in info)
            {
                userDetails.Add(new EmployeeDetails
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    EmailID = item.EmailID,
                    GId = item.GId,
                    LastWorkingDate = item.LastWorkingDate,
                    TeamName = item.TeamName,
                    SponsorName =item.SponsorName,
                    SponsorEmailID =item.SponsorEmailID,
                    SponsorDepartment = item.SponsorDepartment,
                    SponsorGId = item.SponsorGId
                });
            }
            return userDetails;
        }

        public List<Teams> RetrieveAllSponsorDetails()
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

        public List<EmployeeDetails> RetrieveAllActivationGid()
        {
            List<EmployeeDetails> activationDetails = new List<EmployeeDetails>();
            var details = _context.ActivationWorkflow.ToList();
            foreach (var item in details)
            {
                activationDetails.Add(new EmployeeDetails
                {
                    GId = item.GId,
                    
                });
            }
            return activationDetails;
        }

        public async Task<bool> AddEmployeeData(string firstName, string lastName, string gId, string email, DateTime lastWorkingDate, string teamsName, string sponsorName, string sponsorEmailId, string sponsorDepartment ,string sponsorGID)
        //review change make parameters as class
        {
            bool databaseUpdateStatus = false;
            EmployeeDetails employee = new EmployeeDetails()
            {
                FirstName = firstName,
                LastName = lastName,
                GId = gId,
                EmailID = email,
                LastWorkingDate = lastWorkingDate,
                TeamName = teamsName,
                SponsorName = sponsorName,
                SponsorEmailID = sponsorEmailId,
                SponsorDepartment = sponsorDepartment,
                SponsorGId = sponsorGID
            };
            var check = _context.DeactivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.GId == gId)
                {
                    _context.Remove(_context.DeactivationWorkflow.Single(a => a.GId == gId));
                    _context.SaveChanges();
                }
            }

            _context.Add(employee);
            databaseUpdateStatus = _context.SaveChanges() == 1 ? true : false;
            return databaseUpdateStatus;
        }


        public async Task<bool> AddActivationEmployeeData(string firstName, string lastName, string siemensEmailId, string siemensgId, string team, string sponsorName, string sponsorEmailId, string sponsordepartment, string sponsorGID, string reportingManagerEmailId, string employeeRole, string gender, DateTime dob, string pob, string address, string phoneNo, string nationality)
        //review change make parameters as class
        {
            bool databaseUpdateStatus = false;
            EmployeeDetails employeeActivate = new EmployeeDetails()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailID = siemensEmailId,
                GId = siemensgId,
                TeamName = team,
                SponsorName = sponsorName,
                SponsorEmailID = sponsorEmailId,
                SponsorGId= sponsorGID,
                SponsorDepartment = sponsordepartment,
                ReportingManagerEmail = reportingManagerEmailId,
                Role= employeeRole,
                Gender = gender,
                DateOfBirth = dob,
                PlaceOfBirth = pob,
                Address = address,
                PhoneNo = phoneNo,
                Nationality = nationality
            };
            var check = _context.ActivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.SponsorGId == siemensgId)
                {
                    _context.Remove(_context.ActivationWorkflow.Single(a => a.SponsorGId == siemensgId));
                    _context.SaveChanges();
                }
            }

            _context.Add(employeeActivate);
            databaseUpdateStatus = _context.SaveChanges() == 1 ? true : false;
            return databaseUpdateStatus;
        }

        public EmployeeDetails RetrieveEmployeeDataBasedOnGid(string gId)
        {
            var details = _context.DeactivationWorkflow.ToList();
            foreach (var item in details)
            {
                if (item.GId == gId)
                {
                    return item;
                }
            }
            return new EmployeeDetails();

        }

        public EmployeeDetails RetrieveActivationDataBasedOnGid(string gId)
        {
            var details = _context.ActivationWorkflow.ToList();
            foreach (var item in details)
            {
                if (item.GId == gId)
                {
                    return item;
                }
            }
            return new EmployeeDetails();

        }

        public bool savepdf(byte[] pdf, string gid)
        
        {
            var check = _context.ActivationWorkflow.ToList();
            foreach (var i in check)
            {
                if (i.GId == gid)
                {
                    i.ActivationWorkFlowPdfAttachment = pdf;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }

}
