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
	class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
	{
		public void Configure(EntityTypeBuilder<SavedJob> builder)
		{

				builder.HasIndex(s => new { s.SeekerId, s.JobId })
					  .IsUnique();

				builder.HasOne(s => s.Job)
					  .WithMany() 
					  .HasForeignKey(s => s.JobId)
					  .OnDelete(DeleteBehavior.Cascade);

				builder.HasOne(s => s.Seeker)
					  .WithMany() 
					  .HasForeignKey(s => s.SeekerId)
					  .OnDelete(DeleteBehavior.Cascade);

		}
	}
}

