using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        

        public async Task<string> AddInfoToExcel( string name, string gid , string bithday, string email)
        {
            string endpoint = "https://graph.microsoft.com/v1.0/me/drive/items/01XX5ZT435IY27GNKC4JFZ6RMKZR6FIUAL/workbook/worksheets('sheet1')/tables('Table1')/rows/add";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    // Populate UserInfoRequest object
                    string[] userInfo = { name, gid, bithday, email};
                    string[][] userInfoArray = { userInfo };
                    ExcelPostRequest userInfoRequest = new ExcelPostRequest();
                    userInfoRequest.index = null;
                    userInfoRequest.values = userInfoArray;

                    string accessToken = "eyJ0eXAiOiJKV1QiLCJub25jZSI6Il9mQUY3ajNyay1RUC1NTWVya19RRTRHOWtfcWZ0SXd2dUFhb0toOG41SWciLCJhbGciOiJSUzI1NiIsIng1dCI6Imwzc1EtNTBjQ0g0eEJWWkxIVEd3blNSNzY4MCIsImtpZCI6Imwzc1EtNTBjQ0g0eEJWWkxIVEd3blNSNzY4MCJ9.eyJhdWQiOiIwMDAwMDAwMy0wMDAwLTAwMDAtYzAwMC0wMDAwMDAwMDAwMDAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8zOGFlM2JjZC05NTc5LTRmZDQtYWRkYS1iNDJlMTQ5NWQ1NWEvIiwiaWF0IjoxNjM1ODI5NDIzLCJuYmYiOjE2MzU4Mjk0MjMsImV4cCI6MTYzNTgzNTAyOSwiYWNjdCI6MCwiYWNyIjoiMSIsImFjcnMiOlsidXJuOnVzZXI6cmVnaXN0ZXJzZWN1cml0eWluZm8iXSwiYWlvIjoiQVNRQTIvOFRBQUFBZnY4bjNBcFhXOXllT3k2VU1pV2FucEpud09jUGF3SHVhcU9uTmJEa3VTQT0iLCJhbXIiOlsid2lhIl0sImFwcF9kaXNwbGF5bmFtZSI6IkdyYXBoIEV4cGxvcmVyIiwiYXBwaWQiOiJkZThiYzhiNS1kOWY5LTQ4YjEtYThhZC1iNzQ4ZGE3MjUwNjQiLCJhcHBpZGFjciI6IjAiLCJkZXZpY2VpZCI6IjVlYzM5ODI4LWQ1ZTMtNDJhZi1hOTRlLTMzZGNhMjNmZjg4NSIsImZhbWlseV9uYW1lIjoiU3VuaWwiLCJnaXZlbl9uYW1lIjoiQW1uc2h1bWFuIiwiaWR0eXAiOiJ1c2VyIiwiaW5fY29ycCI6InRydWUiLCJpcGFkZHIiOiIxNjUuMjI1LjEyMi4yNDIiLCJuYW1lIjoiU3VuaWwsIEFtbnNodW1hbiAoQURWIEQgQUEgRFRTIEFVIFJEUyBDTlgpIiwib2lkIjoiOTFmMmNjOTEtNDk5ZC00ODBjLWI4NTQtMDY5MzhkODQwMGY3Iiwib25wcmVtX3NpZCI6IlMtMS01LTIxLTEyNjQzMjY2Ni0xMjcwOTEzOTI2LTM2NzkxNTM0MTMtNDE0NDU3OSIsInBsYXRmIjoiMyIsInB1aWQiOiIxMDAzMjAwMTZCNjM2QUVGIiwicmgiOiIwLkFUd0F6VHV1T0htVjFFLXQyclF1RkpYVldyWElpOTc1MmJGSXFLMjNTTnB5VUdROEFKcy4iLCJzY3AiOiJBdWRpdExvZy5SZWFkLkFsbCBEaXJlY3RvcnkuQWNjZXNzQXNVc2VyLkFsbCBEaXJlY3RvcnkuUmVhZC5BbGwgRGlyZWN0b3J5LlJlYWRXcml0ZS5BbGwgRmlsZXMuUmVhZCBGaWxlcy5SZWFkLkFsbCBGaWxlcy5SZWFkV3JpdGUgTWFpbC5SZWFkIG9wZW5pZCBwcm9maWxlIFNpdGVzLk1hbmFnZS5BbGwgU2l0ZXMuUmVhZC5BbGwgU2l0ZXMuUmVhZFdyaXRlLkFsbCBVc2VyLlJlYWQgZW1haWwgRmlsZXMuUmVhZFdyaXRlLkFsbCIsInNpZ25pbl9zdGF0ZSI6WyJkdmNfbW5nZCIsImR2Y19kbWpkIiwiaW5rbm93bm50d2siXSwic3ViIjoiSzRrU0tHaTZmSVQtZHA5V2I5RE9sNnhNbU5zd0JvaXZrRG5nZ0hUY1JmSSIsInRlbmFudF9yZWdpb25fc2NvcGUiOiJFVSIsInRpZCI6IjM4YWUzYmNkLTk1NzktNGZkNC1hZGRhLWI0MmUxNDk1ZDU1YSIsInVuaXF1ZV9uYW1lIjoiYW1uc2h1bWFuLnN1bmlsQHNpZW1lbnMuY29tIiwidXBuIjoiYW1uc2h1bWFuLnN1bmlsQHNpZW1lbnMuY29tIiwidXRpIjoiWG5iVlJBXzJSRUdTTkRaSzQ3MVlBQSIsInZlciI6IjEuMCIsIndpZHMiOlsiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il0sInhtc19zdCI6eyJzdWIiOiJFSGtURl9CTnlwQThvOGt5OVdCRXhESnBEUXFqYnVabzMwWUowNXNqTlRnIn0sInhtc190Y2R0IjoxMzkxMTcxMzIwfQ.oUpeiVuH3zElouM2C6yCaoODra_sT3ar-yAr9UX2bqUuwI0HR0iV_LEgNAuND0XGqXog3Y-ewtjnI3KtRMIzwXGQRqwuem_ZbVBb1Qu8f-1aza5U5dkPbzWK9rZ9SnrZIjmwOC0O0oe-N0Woy3VRv8jjhPM88DbZRdeF3NQu-kBpWBEfpZvnBCAvGTGc49KJCIfrADTK0E9UX0sqBKqD4-keWbt7zWbB1G2Ev5F-qDVP5DT0TqiGAfYEEhoVzY5qWoQFbnvMCOck3tt7MyrlFWlXgdMvPv0xx0zyVkY2HmMeSvjOGkcSsw7qIQqJqOB22Rz6dIhsaUf34BjJpax49A";
                    // Serialize the information in the UserInfoRequest object
                    string jsonBody = JsonConvert.SerializeObject(userInfoRequest);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); 

                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return "true";
                        }
                        return response.ReasonPhrase;
                    }
                }
            }
        }

        #endregion
    }

}
