using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


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
            try
            {
                if (employeeDetails.isDeactivatedWorkFlow)
                {

                    var deactivatedEmployees = RetrieveAllDeactivatedEmployees();
                    foreach (var deactivatedEmployee in deactivatedEmployees)
                    {
                        if (deactivatedEmployee.GId.ToLower() == employeeDetails.GId.ToLower())
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
                        if (activatedEmployee.GId.ToLower() == employeeDetails.GId.ToLower())
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
            catch
            {
                return false;
            }
        }

        

        public List<EmployeeDetails> RetrieveAllDeactivatedEmployees()
        {
            try
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
            var DeactivationData = _context.DeactivationWorkflow.Find(gId);
            if (DeactivationData == null)
            {
                return false;
            }
            _context.Remove(DeactivationData);
            _context.SaveChanges();
            
            return true;
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

        public bool DeleteActivationDetails(string gId)
        {
            var ActivationData = _context.ActivationWorkflow.Find(gId);
            if (ActivationData == null)
            {
                return false;
            }
            _context.Remove(ActivationData);
            _context.SaveChanges();

            return true;

        }

        public bool SavePdfToDatabase(byte[] pdf, string gId)
        {
            try
            {
                var ActivationData = _context.ActivationWorkflow.Find(gId);
                if (ActivationData == null)
                {
                    return false;
                }
                ActivationData.ActivationWorkFlowPdfAttachment = pdf;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public EmployeeDetails RetrieveDeactivatedEmployeeDataBasedOnGid(string gId)
        {
            var DeactivationData = _context.DeactivationWorkflow.Find(gId);
            if (DeactivationData != null)
            {
                EmployeeDetails employeeDetail = JsonConvert.DeserializeObject<EmployeeDetails>(JsonConvert.SerializeObject(DeactivationData));
                return employeeDetail;
            }
                
                return new EmployeeDetails();
        }

        public List<EmployeeDetails> RetrieveDeactivationWorkFlowBaseonDate(string LastWorkingDate)
        {
            var EmployeesData = RetrieveAllDeactivatedEmployees();
            List<EmployeeDetails> employeeDetailsBasedOnDate = new List<EmployeeDetails>();
            foreach (var employeeData in EmployeesData)
            {
                if (LastWorkingDate == employeeData.LastWorkingDate.ToString())
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
                return (_context.Teams.ToList());
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
