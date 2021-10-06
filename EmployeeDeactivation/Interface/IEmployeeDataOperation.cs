using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeDeactivation.Interface
{
    public interface IEmployeeDataOperation
    {
        bool AddEmployeeData(EmployeeDetails employeeDetails);
        List<EmployeeDetails> RetrieveAllDeactivatedEmployees();
        EmployeeDetails RetrieveDeactivatedEmployeeDataBasedOnGid(string gId);
        string[] GetDeactivatedEmployeeDetails(string gid);
        List<EmployeeDetails> RetrieveAllActivationWorkFlow();
        EmployeeDetails RetrieveActivationDataBasedOnGid(string gId);
        string[] GetReportingEmailIds(string teamName);
        List<Teams> RetrieveAllSponsorDetails();
        bool SavePdfToDatabase(byte[] pdf, string gId);
        bool DeleteDeactivationDetails(string gId);
        bool DeleteActivationDetails(string gId);
        //Task<List<EmployeeDetails>> RetrieveDeactivatedEmployees();
        //List<EmployeeDetails> RetrieveDeactivatedEmployeeDataBasedOnGidAsync(string gId);
        //Task<string[]> GetDeactivatedEmployeeDetails1(string gid);
        //Task<EmployeeDetails> RetrieveDeactivatedEmployeeDataBasedOnGid11(string gId);
        //Task<List<EmployeeDetails>> RetrieveAllDeactivatedEmployees1();

        List<EmployeeDetails> RetrieveDeactivationWorkFlowBaseonDate(string Datee);
    }
}
