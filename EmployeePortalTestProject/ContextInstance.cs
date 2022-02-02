using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Data;
using EmployeeDeactivation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalTestProject
{
    class ContextInstance
    {
        public static EmployeeDeactivationContext databaseEmployeeContext()
        {
            var options = new DbContextOptionsBuilder<EmployeeDeactivationContext>()
            .UseInMemoryDatabase(databaseName: "EmployeeWorkflowDatabases")
            .Options;
            using (var context = new EmployeeDeactivationContext(options))
            {
                context.ActivationWorkflow.Add(new ActivationEmployeeDetails { FirstName = "abhi", LastName = "dd", EmailID = "amnsh@gm.com", GId = "56", TeamName = "it", SponsorFirstName = "dd", SponsorLastName = "ll", SponsorEmailID = "nsh@gm.com", SponsorGId = "899", SponsorDepartment = "ui", ReportingManagerEmail = "dfnsh@gm.com", Role = "intern", Gender = "male", DateOfBirth = DateTime.Now, PlaceOfBirth = "delhi", Address = "delhi", PhoneNo = "1234", Nationality = "Indian", ActivationWorkFlowPdfAttachment = null, ActivationDate = DateTime.Now.ToString() });
                context.ActivationWorkflow.Add(new ActivationEmployeeDetails { FirstName = "amnshu", LastName = "ss", EmailID = "ddfnsh@gm.com", GId = "65", TeamName = "app", SponsorFirstName = "gg", SponsorLastName = "jj", SponsorEmailID = "nyysh@gm.com", SponsorGId = "879", SponsorDepartment = "jj", ReportingManagerEmail = "duinsh@gm.com", Role = "intern", Gender = "male", DateOfBirth = DateTime.Now, PlaceOfBirth = "delhi", Address = "delhi", PhoneNo = "1234678967", Nationality = "Indian", ActivationWorkFlowPdfAttachment = null, ActivationDate = DateTime.Now.ToString() });
                context.Teams.Add(new Teams { TeamName = "IT", SponsorFirstName = "Jhon", SponsorLastName = "WH", SponsorEmailID = "ddfnsh@gm.com", SponsorGID = "67", Department = "it-op", ReportingManagerEmailID = "deensh@gm.com", SivantosPointEmailID = "sivh@gm.com", CmEmailID = "ccnsh@gm.com", CcEmailID = "ccccsh@gm.com", SivantosPointName = "roy" });
                context.Teams.Add(new Teams { TeamName = "app", SponsorFirstName = "Harry", SponsorLastName = "WH", SponsorEmailID = "ddfnsh@gm.com", SponsorGID = "99", Department = "it-com", ReportingManagerEmailID = "deensh@gm.com", SivantosPointEmailID = "sivh@gm.com", CmEmailID = "ccnsh@gm.com", CcEmailID = "ccccsh@gm.com", SivantosPointName = "roy" });
                context.SaveChanges();
            }
            var dbContext = new EmployeeDeactivationContext(options);
            return dbContext;
        }

        public static EmployeeDeactivationContext CreateTesttEmployeeContext()
        {
            var options = new DbContextOptionsBuilder<EmployeeDeactivationContext>()
            .UseSqlServer(@"Server = localhost\\SQLEXPRESS01; Integrated Security = True; Database = EmployeeWorkflowDatabases; Trusted_Connection = True; MultipleActiveResultSets = True;")
            .Options;


            var dbContext = new EmployeeDeactivationContext(options);
            return dbContext;
        }

        public static EmployeeDeactivationContext CreateInMemeoryDatabaseContext()
        {
            var InMemooptions = new DbContextOptionsBuilder<EmployeeDeactivationContext>()
            .UseInMemoryDatabase(databaseName: "EmployeeWorkflowDatabases")
            .Options;


            var dbContext = new EmployeeDeactivationContext(InMemooptions);
            return dbContext;
        }

    }
}
