using JobBoard.Domain.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<UserApplication>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        
        public DbSet<SeekerProfile> SeekerProfiles { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SeekerProfile>().HasKey(p => p.Id);
            builder.Entity<EmployerProfile>().HasKey(e => e.Id);

            builder.Entity<SeekerProfile>()
                .HasOne(s => s.User)
                .WithOne(u=> u.seekerProfile)
                .HasForeignKey<SeekerProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EmployerProfile>()
               .HasOne(s => s.User)
               .WithOne(u=>u.employerProfile)
               .HasForeignKey<EmployerProfile>(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Application>()
                .HasOne(a => a.Applicant)
                .WithMany(s => s.UserApplications)
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Application>()
                .HasOne(a => a.Job)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Skill>()
                .HasOne(s => s.Job)
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Job>()
                .HasOne(j => j.Employer)
                .WithMany(e => e.PostedJobs)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); 
        }

    }
}
