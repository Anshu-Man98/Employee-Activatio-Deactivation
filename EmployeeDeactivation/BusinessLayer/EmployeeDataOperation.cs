using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class EmployeeDataOperation : IEmployeeDataOperation
    {
        private readonly EmployeeDeactivationContext ccontext;
        public EmployeeDataOperation(EmployeeDeactivationContext context)
        {
            ccontext = context;
        }
        #region Employee Data
        public bool AddEmployeeData(EmployeeDetails employeeDetails)
        {
            try
            {
                if (employeeDetails.isDeactivatedWorkFlow)
                {

                    var deactivatedEmployees = RetrieveAllDeactivatedEmployees();
                    foreach (var deactivatedEmployee in deactivatedEmployees)
                    {
                        if (deactivatedEmployee.GId.ToLower() == employeeDetails.GId.ToLower())
                        {
                            ccontext.Remove(ccontext.DeactivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                            ccontext.SaveChanges();
                        }
                    }
                    DeactivationEmployeeDetails employeeDetail = JsonConvert.DeserializeObject<DeactivationEmployeeDetails>(JsonConvert.SerializeObject(employeeDetails));
                    ccontext.Add(employeeDetail);
                }
                else
                {
                    var activatedEmployees = RetrieveAllActivationWorkFlow();
                    foreach (var activatedEmployee in activatedEmployees)
                    {
                        if (activatedEmployee.GId.ToLower() == employeeDetails.GId.ToLower())
                        {
                           ccontext.Remove(ccontext.ActivationWorkflow.Single(a => a.GId == employeeDetails.GId));
                            ccontext.SaveChanges();
                        }
                    }
                    ActivationEmployeeDetails employeeDetail = JsonConvert.DeserializeObject<ActivationEmployeeDetails>(JsonConvert.SerializeObject(employeeDetails));
                    ccontext.Add(employeeDetail);
                }

                return ccontext.SaveChanges() == 1;
            }
            catch
            {
                return false;
            }
        }


        public List<EmployeeDetails> RetrieveAllDeactivatedEmployees()
        {
            try
            {
                var deactivatedEmployees = ccontext.DeactivationWorkflow.ToList();
                List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
                foreach (var item in deactivatedEmployees)
                {
                    EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(item));
                    employeeDetails.Add(employeeDetail);
                }
                return employeeDetails;
            }
            catch(Exception e)
            {
                string fileName = @"C:\Temp\LogError1.txt";


                if (File.Exists(fileName))

                {

                    File.Delete(fileName);

                }

                // Create a new file     

                using (FileStream fs = File.Create(fileName))

                {

                    // Add some text to file    

                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");

                    fs.Write(title, 0, title.Length);
                    byte[] text = new UTF8Encoding(true).GetBytes("ERROR Retrive------------------> " + e.StackTrace);

                    fs.Write(text);
                    return null;

                }
            }
        }

        public bool DeleteDeactivationDetails(string gId)
        {
            var deactivatedEmployees = RetrieveAllDeactivatedEmployees();
            foreach (var deactivatedEmployee in deactivatedEmployees)
            {
                if (deactivatedEmployee.GId.ToLower() == gId.ToLower())
                {
                    ccontext.Remove(ccontext.DeactivationWorkflow.Single(a => a.GId == gId));
                    ccontext.SaveChanges();
                }
            }
            return true;
        }

        public List<EmployeeDetails> RetrieveAllActivationWorkFlow()
        {
            var activatedEmployees = ccontext.ActivationWorkflow.ToList();
            List<EmployeeDetails> employeeDetails = new List<EmployeeDetails>();
            foreach (var item in activatedEmployees)
            {
                EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(item));
                employeeDetails.Add(employeeDetail);
            }
            return employeeDetails;
        }

        public bool DeleteActivationDetails(string gId)
        {
            var activatedEmployees = RetrieveAllActivationWorkFlow();
            foreach (var activatedEmployee in activatedEmployees)
            {
                if (activatedEmployee.GId.ToLower() == gId.ToLower())
                {
                    ccontext.Remove(ccontext.ActivationWorkflow.Single(a => a.GId == gId));
                    ccontext.SaveChanges();
                }
            }
            return true;
        }

        public bool SavePdfToDatabase(byte[] pdf, string gId)
        {
            try
            {

                var activationWorkflows = ccontext.ActivationWorkflow.ToList();
                foreach (var activatedWorkflow in activationWorkflows)
                {
                    if (activatedWorkflow.GId.ToLower() == gId.ToLower())
                    {
                        activatedWorkflow.ActivationWorkFlowPdfAttachment = pdf;
                        ccontext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public EmployeeDetails RetrieveDeactivatedEmployeeDataBasedOnGid(string gId)
        {
            
                var employeeDetails = RetrieveAllDeactivatedEmployees();
                foreach (var employee in employeeDetails)
                {
                    if (employee.GId.ToLower() == gId.ToLower())
                    {
                        return employee;
                    }
                }
                return new EmployeeDetails();
        }

        public List<EmployeeDetails> RetrieveDeactivationWorkFlowBaseonDate(string Datee)
        {
            var EmployeesData = RetrieveAllDeactivatedEmployees();
            List<EmployeeDetails> employeeDetailsBasedOnDate = new List<EmployeeDetails>();
            foreach (var employeeData in EmployeesData)
            {
                if (Datee == employeeData.LastWorkingDate.ToString())
                {
                    EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(employeeData));
                    employeeDetailsBasedOnDate.Add(employeeDetail);
                }
            }
            return employeeDetailsBasedOnDate;
        }

        public string[] GetDeactivatedEmployeeDetails(string gid)
        {
            var deactivationDetails = RetrieveAllDeactivatedEmployees();
            foreach (var item in deactivationDetails)
            {
                if (item.GId.ToLower() == gid.ToLower())
                {
                    return new string[] { item.EmailID, item.TeamName, item.CcEmailId, item.FromEmailId, item.SponsorEmailID};

                }

            }

            return new string[] { };
        }

        public EmployeeDetails RetrieveActivationDataBasedOnGid(string gId)
        {
            var allActivatedWorkFlows = RetrieveAllActivationWorkFlow();
            foreach (var item in allActivatedWorkFlows)
            {
                if (item.GId.ToLower() == gId.ToLower())
                {
                    return item;
                }
            }
            return new EmployeeDetails();

        }
        #endregion

        #region SponsorData

        public string[] GetReportingEmailIds(string teamName)
        {
            try
            {
                var teamDetails = RetrieveAllSponsorDetails();
                foreach (var item in teamDetails)
                {
                    if (item.TeamName.ToLower() == teamName.ToLower())
                    {
                        return new string[] { item.CmEmailID, item.SivantosPointEmailID, item.CcEmailID, item.ReportingManagerEmailID };
                    }
                }
                return new string[] { };
            }
            catch
            {
                return null;
            }
        }
        public List<Teams> RetrieveAllSponsorDetails()

        {
            try
            {
                return (ccontext.Teams.ToList());
            }
            catch (Exception e)
            {


                string fileName = @"C:\Temp\LogError1.txt";


                if (File.Exists(fileName))

                {

                    File.Delete(fileName);

                }

                // Create a new file     

                using (FileStream fs = File.Create(fileName))

                {

                    // Add some text to file    

                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");

                    fs.Write(title, 0, title.Length);
                    byte[] text = new UTF8Encoding(true).GetBytes("ERROR------------------> " + e.StackTrace);

                    fs.Write(text);
                    return null;

                }
            }

        }

        #endregion
    }

}
