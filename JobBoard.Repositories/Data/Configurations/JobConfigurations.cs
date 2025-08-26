using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Data.Configurations
{
	class JobConfigurations : IEntityTypeConfiguration<Job>
	{
		public void Configure(EntityTypeBuilder<Job> builder)
		{
			builder.Property(j => j.Title)
				 .IsRequired()
				 .HasMaxLength(100);

			builder.Property(j => j.Salary).HasColumnType("decimal(18,2)");

			builder.Property(j => j.WorkplaceType).HasConversion<string>();

			builder.Property(j => j.JobType).HasConversion<string>();

			builder.Property(j => j.EducationLevel).HasConversion<string>();

			builder.Property(j => j.ExperienceLevel).HasConversion<string>();

			builder.HasOne(j => j.Employer)
				.WithMany(e => e.PostedJobs)
				.HasForeignKey(j => j.EmployerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(j => j.Skills)
				   .WithMany(s => s.Jobs)
				   .UsingEntity(j => j.ToTable("JobSkills"));
			
			builder.HasMany(j => j.Categories)
				   .WithMany(s => s.Jobs)
				   .UsingEntity(j => j.ToTable("JobCategories"));

            builder.Property(j => j.IsDeleted).HasDefaultValue(false);
            builder.HasQueryFilter(j => !j.IsDeleted);

        }
	}
}
