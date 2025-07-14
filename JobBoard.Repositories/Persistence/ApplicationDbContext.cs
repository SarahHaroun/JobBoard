using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Repositories.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        
        public DbSet<SeekerProfile> SeekerProfiles { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SeekerProfile>().HasKey(p => p.Id);
            builder.Entity<EmployerProfile>().HasKey(e => e.Id);
        }
    }
}
