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
    public class SeekerEducationConfiguration : IEntityTypeConfiguration<SeekerEducation>
    {
        public void Configure(EntityTypeBuilder<SeekerEducation> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Major).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.Faculty).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.University).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.Location).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.EducationLevel).HasConversion<string>();
            builder.Property(e => e.GPA).IsRequired(false);
            builder.Property(e => e.Date).IsRequired(false);

        }
    }
}
