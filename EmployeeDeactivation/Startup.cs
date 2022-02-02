using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Data;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.IO;
using System.Text;

namespace EmployeeDeactivation
{

    public class Startup
    {
        //private IPdfDataOperation _pdfDataOperation;
        public Startup(IConfiguration configuration)
        {
            try
            {
                Configuration = configuration;
            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorIConfiguration.txt";


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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                //services.AddHttpsRedirection(options =>
                //{
                //    options.HttpsPort = 443;
                //});

                services.Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

                //var server = Configuration["DBServer"] ?? "localhost";
                //var port = Configuration["DBPort"] ?? "1433";
                //var user = Configuration["DBUser"] ?? "SA";
                //var password = Configuration["DBPassword"] ?? "1234@Abcd";
                //var database = Configuration["Database"] ?? "EmployeeDeactivationContext-ad1fce0f-4b85-42a1-9c48-4572137b7d8d";




                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                services.AddAuthentication(AzureADDefaults.AuthenticationScheme).AddAzureAD(options => Configuration.Bind("AzureAd", options));


                services.AddDbContext<EmployeeDeactivationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EmployeeDeactivationContext")));
                //options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID ={user};Password={password}"));

                services.AddScoped<IEmployeeDataOperation, EmployeeDataOperation>();
                services.AddScoped<IPdfDataOperation, PdfDataOperation>();
                services.AddScoped<IAdminDataOperation, AdminDataOperation>();
                services.AddScoped<IManagerApprovalOperation, ManagerApprovalOperation>();
                services.AddScoped<IEmailOperation, EmailOperations>();
                services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
                {
                    options.Authority = options.Authority + "/v2.0/";
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.RequireHttpsMetadata = true;
                });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("Manager", policyBuilder => policyBuilder.RequireClaim("groups", "2dc589b5-96a0-4453-b8dc-eb4b372b1092"));
                    options.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim("groups", "7ffec22d-7987-493b-aa3b-e24ca29ba04b"));
                    options.AddPolicy("Admin&Manager", policyBuilder => policyBuilder.RequireClaim("groups", "7ffec22d-7987-493b-aa3b-e24ca29ba04b", "2dc589b5-96a0-4453-b8dc-eb4b372b1092"));
                });


                services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));


                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                services.AddControllersWithViews(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }).AddMicrosoftIdentityUI();

            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorIServiceCollection.txt";


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



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next();
                });
                app.UseAuthentication();
                app.UseCookiePolicy();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Employees}/{action=EmployeeDeactivationForm}/{id?}");
                });


            }
            catch (Exception e)
            {
                string fileName = @"C:\Temp\ErrorConfigure.txt";


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

    }

}