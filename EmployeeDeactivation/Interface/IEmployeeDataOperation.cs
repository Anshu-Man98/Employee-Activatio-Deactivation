﻿using EmployeeDeactivation.Models;
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
    }
}
