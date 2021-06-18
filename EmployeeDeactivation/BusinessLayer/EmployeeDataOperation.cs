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

        public bool AddEmployeeData(EmployeeDetails employeeDetails, bool isDeactivatedWorkFlow)
        {
            if (isDeactivatedWorkFlow)
            {
                var deactivatedEmployees = _context.DeactivationWorkflow.ToList();
                foreach (var deactivatedEmployee in deactivatedEmployees)
                {
                    if (deactivatedEmployee.GId == employeeDetails.GId)
                    {
                        _context.Remove(_context.DeactivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                var activatedEmployees = _context.ActivationWorkflow.ToList();
                foreach (var activatedEmployee in activatedEmployees)
                {
                    if (activatedEmployee.GId == employeeDetails.GId)
                    {
                        _context.Remove(_context.ActivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                        _context.SaveChanges();
                    }
                }
            }
            _context.Add(employeeDetails);
            return _context.SaveChanges() == 1;
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
