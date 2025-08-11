using JobBoard.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        
        public DbSet<SeekerProfile> SeekerProfiles { get; set; }
        public DbSet<SeekerEducation> SeekerEducations { get; set; }
        public DbSet<SeekerExperience> SeekerExperiences { get; set; }
        public DbSet<SeekerCertificate> SeekerCertificates { get; set; }
        public DbSet<SeekerInterest> seekerInterests { get; set; }
        public DbSet<SeekerTraining> SeekerTrainings { get; set; }

        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<AIEmbedding> AIEmbeddings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
