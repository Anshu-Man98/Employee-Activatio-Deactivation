using NUnit.Framework;
using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Data;
using EmployeeDeactivation.Models;
using NSubstitute;
using EmployeeDeactivation.Interface;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortalTestProject
{

    [TestFixture]
    public class EmployeeOperationTest1
    {
        
        private EmployeeDataOperation _employeeData;
        [SetUp]
        public void Setup()
        {
            _employeeData = new EmployeeDataOperation(ContextInstance.databaseEmployeeContext());
        }
        
        [Test]
        public void Delete_Activated_employee_Details()
        {
            //ARRANGE 

            //Act
            var result = _employeeData.DeleteActivationDetails("65");
            //Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void Retrive_Activated_employee_Details_Based ()
        {
            //ARRANGE 

            //Act
            var result = _employeeData.RetrieveActivationDataBasedOnGid("56");
            //Assert
            Assert.AreEqual(result.GId,"56");
        }

        [Test]
        public void SavePDF()
        {
            //ARRANGE 
            byte[] bytesData = Encoding.Default.GetBytes("ABC123");
            //Act
            var pdfResult = _employeeData.SavePdfToDatabase(bytesData,"65");
            //Assert
            Assert.IsTrue(pdfResult);
        }

        [Test]
        public void Retrive_all_Activation_Workflow()
        {
            //ARRANGE 
            List<EmployeeDetails> emp = _employeeData.RetrieveAllActivationWorkFlow();
            //Assert
            Assert.AreEqual(2, emp.Count);
        }

        
    }
}
