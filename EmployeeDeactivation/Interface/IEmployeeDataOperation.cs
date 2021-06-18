using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeDeactivation.Interface
{
    public interface IEmployeeDataOperation
    {
        List<Teams> RetrieveAllSponsorDetails();
        bool AddEmployeeData(EmployeeDetails employeeDetails, bool isDeactivatedWorkFlow);
        EmployeeDetails RetrieveEmployeeDataBasedOnGid(string gId);
        EmployeeDetails RetrieveActivationDataBasedOnGid(string gId);
        string GetReportingManagerEmailId(string teamName);
        string GetSponsorEmailId(string SponsorGid);
        List<EmployeeDetails> SavedEmployeeDetails();
        bool savepdf(byte[] pdf, string gid);
        string GetEmployeeEmailId(string gid);
    }
}
