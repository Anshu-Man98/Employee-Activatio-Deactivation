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
        string GetDeactivatedEmployeeEmailId(string gid);
        List<EmployeeDetails> RetrieveAllActivationWorkFlow();
        EmployeeDetails RetrieveActivationDataBasedOnGid(string gId);
        string GetReportingManagerEmailId(string teamName);
        string GetSponsorEmailId(string sponsorGid);
        List<Teams> RetrieveAllSponsorDetails();
        bool SavePdfToDatabase(byte[] pdf, string gId);
    }
}
