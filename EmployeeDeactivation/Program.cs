using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeDeactivation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorProgramcs.txt";


                if (File.Exists(fileName))

                {

                    File.Delete(fileName);

                }

                using (FileStream fs = File.Create(fileName))

                {

                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");

                    fs.Write(title, 0, title.Length);
                    byte[] text = new UTF8Encoding(true).GetBytes("ERROR Retrive from ------------------> " + e.StackTrace);

                    fs.Write(text);


                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
