using JobBoard.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Data.Configurations
{
	class SeekerProfileConfigurations : IEntityTypeConfiguration<SeekerProfile>
	{
		public void Configure(EntityTypeBuilder<SeekerProfile> builder)
		{
			builder.HasKey(s => s.Id);    
			
			builder.HasOne(s => s.User)
					.WithOne()
					.HasForeignKey<SeekerProfile>(s => s.UserId)
					.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(s => s.Skills)
				    .WithMany(sk => sk.Seekers)
					.UsingEntity(j => j.ToTable("SeekerSkills"));

		}
	}
}
