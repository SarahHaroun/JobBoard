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
	class SeekerProfileConfigurations : IEntityTypeConfiguration<SeekerProfile>
	{
		public void Configure(EntityTypeBuilder<SeekerProfile> builder)
		{
			builder.HasKey(s => s.Id);

            // string length limits and nullable
            builder.Property(s => s.Name).HasMaxLength(200).IsRequired(false);
            builder.Property(s => s.Title).HasMaxLength(200).IsRequired(false);
            builder.Property(s => s.Address).HasMaxLength(500).IsRequired(false);
            builder.Property(s => s.Summary).HasMaxLength(2000).IsRequired(false);
            builder.Property(s => s.CV_Url).HasMaxLength(1000).IsRequired(false);
            builder.Property(s => s.ProfileImageUrl).HasMaxLength(1000).IsRequired(false);
            builder.Property(s => s.Gender).HasConversion<string>();


            // FK to Identity user (required and unique because one-to-one)
            builder.Property(s => s.UserId)
                   .IsRequired();

            builder.HasIndex(s => s.UserId)
                   .IsUnique();

            builder.HasOne(s => s.User)
					.WithOne(u=>u.seekerProfile)
					.HasForeignKey<SeekerProfile>(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(s => s.Skills)
				    .WithMany(sk => sk.Seekers)
					.UsingEntity(j => j.ToTable("SeekerSkills"));

            // experiences (one-to-many)
            builder.HasMany(s => s.SeekerExperiences)
                   .WithOne(e => e.SeekerProfile)
                   .HasForeignKey(e => e.SeekerProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            // education (one-to-many)
            builder.HasMany(s => s.SeekerEducations)
                   .WithOne(e => e.SeekerProfile)
                   .HasForeignKey(e => e.SeekerProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            //intersts (one-to-many)
            builder.HasMany(s=> s.seekerInterests)
                .WithOne(i => i.SeekerProfile)
                .HasForeignKey(i => i.SeekerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            //trainings (one-to-many)
            builder.HasMany(s => s.SeekerTraining)
                .WithOne(t => t.SeekerProfile)
                .HasForeignKey(t => t.SeekerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            //certificates  (one-to-many)
            builder.HasMany(s => s.seekerCertificates)
                .WithOne(c => c.SeekerProfile)
                .HasForeignKey(c => c.SeekerProfileId)
                .OnDelete(DeleteBehavior.Cascade);

        }
	}
}
