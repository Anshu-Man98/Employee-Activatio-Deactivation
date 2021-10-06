using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDeactivation.Data
{

    public class EmployeeDeactivationContext : DbContext
    {

        public EmployeeDeactivationContext(DbContextOptions<EmployeeDeactivationContext> options)
            : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception e)
            {


                string fileName = @"C:\Temp\LogError.txt";


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


                }
            }

        }
        public DbSet<Models.DeactivationEmployeeDetails> DeactivationWorkflow { get; set; }
        public DbSet<Models.Teams> Teams { get; set; }
        public DbSet<Models.ManagerApprovalStatus> ManagerApprovalStatus { get; set; }
        public DbSet<Models.ActivationEmployeeDetails> ActivationWorkflow { get; set; }
        public DbSet<Models.Tokens> Tokens { get; set; }
        public DbSet<Models.DeactivationStatus> DeactivationStatus { get; set; }
        public DbSet<Models.ActivationStatus> ActivationStatus { get; set; }



    }
}
