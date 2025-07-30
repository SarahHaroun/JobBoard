using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Data.Configurations
{
	class EmployerProfileConfigurations : IEntityTypeConfiguration<EmployerProfile>
	{
		public void Configure(EntityTypeBuilder<EmployerProfile> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(e => e.User)
			   .WithOne(u=>u.employerProfile)
			   .HasForeignKey<EmployerProfile>(e => e.UserId)
			   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
