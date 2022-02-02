using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EmployeePortalTestProject
{
    [TestFixture]
    class AdminOperationTest
    {
        private IEmployeeDataOperation _empOperation;


            [SetUp]
            public void Setup()
            {
            _empOperation = Substitute.For<IEmployeeDataOperation>();
            }

        [Test]
        public void Retrieve_Sponsor_Details_Test()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
            var AdminOperation = new AdminDataOperation(_empOperation, _context);
            //Act

            //Assert
            Assert.AreEqual(2, AdminOperation.RetrieveSponsorDetails().Count());
        }

        [Test]
        public void Retrieve_Sponsor_Details_BasedONGid_Test()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
            var AdminOperation = new AdminDataOperation(_empOperation, _context);
            //Act

            var result = AdminOperation.RetrieveSponsorDetailsAccordingToTeamName("app");
            //Assert

            Assert.AreEqual(result.SponsorGID,"99");
        }

        [Test]
        public void ADD_Sponsor_Details_Test()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
              var AdminOperation = new AdminDataOperation(_empOperation, _context);
            var TeamData = DummyTeamData();
            //Act

            //Assert

            Assert.IsTrue(AdminOperation.AddSponsorData(TeamData));
        }

        [Test]
        public void Delete_Sponsor_Details_Test()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
            var AdminOperation = new AdminDataOperation(_empOperation, _context);
            //Act

            //Assert

            Assert.IsTrue(AdminOperation.DeleteSponsorData("app"));
        }

        [Test]
        public void Admin_GetActivatedEmpData()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
            var AdminOperation = new AdminDataOperation(_empOperation, _context);
            var DummyListEmp = DummyActivationEmpData();

            //Act
            _empOperation.RetrieveAllActivationWorkFlow().Returns(DummyListEmp);
            var result = AdminOperation.ActivationEmployeeData();
            //Assert

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Admin_GetDeactivatedEmpData()
        {
            //ARRANGE 
            var _context = ContextInstance.databaseEmployeeContext();
            var AdminOperation = new AdminDataOperation(_empOperation, _context);
            var DummyListEmp = DummyDeactivationEmpData();
            var ExpectedResult = 1;
            //Act
            _empOperation.RetrieveAllDeactivatedEmployees().Returns(DummyListEmp);
            var result = AdminOperation.DeactivationEmployeeData();
            //Assert

            Assert.AreEqual(ExpectedResult, result.Count());
        }



        private Teams DummyTeamData()
        {
            return new Teams
            {
            TeamName = "IT-app",
            SponsorFirstName = "Amnshu",
            SponsorLastName = "SUnil",
            SponsorEmailID = "asd@asdhj.com",
            SponsorGID = "hh67",
            Department = "ITS",
            ReportingManagerEmailID = "askd@jahsd.ds",
            SivantosPointEmailID = "fjksdf.dsad@dsd.sa",
            CcEmailID= "fjksdf.dsad@dsd.sa",
            CmEmailID = "fjksdf.dsad@dsd.sa",
            SivantosPointName = "asd"
            };
        }

        private List<EmployeeDetails> DummyActivationEmpData()
        {
            var EmpData = new List<EmployeeDetails>();

            var employeeDetail = new EmployeeDetails()
            {
            FirstName = "abhi",
            LastName = "dd",
            EmailID = "amnsh@gm.com",
            GId = "56",
            TeamName = "it",
            SponsorFirstName = "dd",
            SponsorLastName = "ll",
            SponsorEmailID = "nsh@gm.com",
            SponsorGId = "899",
            SponsorDepartment = "ui",
            ReportingManagerEmail = "dfnsh@gm.com",
            Role = "intern",
            Gender = "male",
            DateOfBirth = DateTime.Now,
            PlaceOfBirth = "delhi",
            Address = "delhi",
            PhoneNo = "1234",
            Nationality = "Indian",
            ActivationWorkFlowPdfAttachment = null,
            ActivationDate = DateTime.Now.ToString()
            };

            EmpData.Add(employeeDetail);
            return EmpData;
        }

        private List<EmployeeDetails> DummyDeactivationEmpData()
        {
            var EmpData = new List<EmployeeDetails>();

            var employeeDetail = new EmployeeDetails()
            {
            FirstName = "abhi",
            LastName ="SS",
            EmailID = "amnsh@gm.com",        
            GId ="61",
            LastWorkingDate = DateTime.Now ,
            TeamName  = "jj",
            SponsorFirstName = "jj",
            SponsorLastName = "jj",
            SponsorEmailID = "fghd@dfg.fhg",
            SponsorDepartment = "hh",
            SponsorGId = "o90",
            ToEmailId = "gdfg@dsf.ghj",
            FromEmailId = "fghfg@dff.jh",
            CcEmailId = "ghjgh@fsdf.yui",
            };

            EmpData.Add(employeeDetail);
            return EmpData;
        }
    }
}
