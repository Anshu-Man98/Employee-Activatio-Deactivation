using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDeactivation.Data
{
    public class EmployeeDeactivationContext : DbContext
    {
        public EmployeeDeactivationContext (DbContextOptions<EmployeeDeactivationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Models.EmployeeDetails> DeactivationWorkflow { get; set; }
        public DbSet<Models.Teams> Teams { get; set; }
        public DbSet<Models.ManagerApprovalStatus> ManagerApprovalStatus { get; set; }
        public DbSet<Models.EmployeeDetails> ActivationWorkflow { get; set; }
    }
}
