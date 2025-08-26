using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Data.Configurations
{
	class ApplicationConfigurations : IEntityTypeConfiguration<Application>
	{
		public void Configure(EntityTypeBuilder<Application> builder)
		{
			builder.HasOne(a => a.Job)
				.WithMany(j => j.JobApplications)
				.HasForeignKey(a => a.JobId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Applicant)
				.WithMany(s => s.UserApplications)
				.HasForeignKey(a => a.ApplicantId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
