﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EmployeeDeactivation.Models;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.BusinessLayer;
using EmployeeDeactivation.Data;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EmployeeDeactivation
{
    public class Startup
    {
        //private IPdfDataOperation _pdfDataOperation;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });

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




            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme).AddAzureAD(options => Configuration.Bind("AzureAd", options)).AddCookie();

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
                options.AddPolicy("Manager", policyBuilder => policyBuilder.RequireClaim("groups", "cac297fa-e30d-4085-bf10-06e38a94b3f7"));
                options.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim("groups", "1f6eeeba-fa1a-4d51-bbba-f0bf6b2a7d4f"));
                options.AddPolicy("Admin&Manager", policyBuilder => policyBuilder.RequireClaim("groups", "cac297fa-e30d-4085-bf10-06e38a94b3f7", "1f6eeeba-fa1a-4d51-bbba-f0bf6b2a7d4f"));
            });


            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));


            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env )
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
            app.Use((context, next) => {
                context.Request.Scheme = "https";
                return next();
            });
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(
                routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Employees}/{action=EmployeeDeactivationForm}/{id?}");
            });
        }
    }
}
