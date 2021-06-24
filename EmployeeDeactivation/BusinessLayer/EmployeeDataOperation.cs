using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Newtonsoft.Json;
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
        #region Employee Data
        public bool AddEmployeeData(EmployeeDetails employeeDetails)
        {
            if (employeeDetails.isDeactivatedWorkFlow)
            {
                var deactivatedEmployees = RetrieveAllDeactivatedEmployees();
                foreach (var deactivatedEmployee in deactivatedEmployees)
                {
                    if (deactivatedEmployee.GId == employeeDetails.GId)
                    {
                        _context.Remove(_context.DeactivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                        _context.SaveChanges();
                    }
                }
                DeactivationEmployeeDetails employeeDetail = JsonConvert.DeserializeObject<DeactivationEmployeeDetails>(JsonConvert.SerializeObject(employeeDetails));
                _context.Add(employeeDetail);
            }
            else
            {
                var activatedEmployees = RetrieveAllActivationWorkFlow();
                foreach (var activatedEmployee in activatedEmployees)
                {
                    if (activatedEmployee.GId == employeeDetails.GId)
                    {
                        _context.Remove(_context.ActivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                        _context.SaveChanges();
                    }
                }
            ActivationEmployeeDetails employeeDetail = JsonConvert.DeserializeObject<ActivationEmployeeDetails>(JsonConvert.SerializeObject(employeeDetails));
                _context.Add(employeeDetail);
            }
           
            return _context.SaveChanges() == 1;
        }

        public List<EmployeeDetails> RetrieveAllDeactivatedEmployees()
        {

            var deactivatedEmployees = _context.DeactivationWorkflow.ToList();
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            foreach (var item in deactivatedEmployees)
            {
                EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(item));
                employeeDetails.Add(employeeDetail);
            }
            return employeeDetails;
        }

        public List<EmployeeDetails> RetrieveAllActivationWorkFlow()
        {
            var activatedEmployees = _context.ActivationWorkflow.ToList();
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            foreach (var item in activatedEmployees)
            {
                EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(item));
                employeeDetails.Add(employeeDetail);
            }
            return employeeDetails;
        }


        public bool SavePdfToDatabase(byte[] pdf, string gId)
        {

            var activationWorkflows = _context.ActivationWorkflow.ToList();
            foreach (var activatedWorkflow in activationWorkflows)
            {
                if (activatedWorkflow.GId == gId)
                {
                    activatedWorkflow.ActivationWorkFlowPdfAttachment = pdf;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
     
        public EmployeeDetails RetrieveDeactivatedEmployeeDataBasedOnGid(string gId)
        {
            var employeeDetails = RetrieveAllDeactivatedEmployees();
            foreach (var employee in employeeDetails)
            {
                if (employee.GId == gId)
                {
                    return employee;
                }
            }
            return new EmployeeDetails();
        }
        public string GetDeactivatedEmployeeEmailId(string gid)
        {
            var deactivationDetails = RetrieveAllDeactivatedEmployees();
            foreach (var item in deactivationDetails)
            {
                if (item.GId == gid)
                {
                    return item.EmailID;
                }
            }
            return "";
        }

        public EmployeeDetails RetrieveActivationDataBasedOnGid(string gId)
        {
            var allActivatedWorkFlows = RetrieveAllActivationWorkFlow();
            foreach (var item in allActivatedWorkFlows)
            {
                if (item.GId == gId)
                {
                    return item;
                }
            }
            return new EmployeeDetails();

        }
        #endregion

        #region SponsorData

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
        public string GetSponsorEmailId(string sponsorGid)
        {
            var teamDetails = RetrieveAllSponsorDetails();
            foreach (var item in teamDetails)
            {
                if (item.SponsorGID == sponsorGid)
                {
                    return item.SponsorEmailID;
                }
            }
            return "";
        }
        public List<Teams> RetrieveAllSponsorDetails()
        {
           return _context.Teams.ToList();
        }

        #endregion
    }

}
