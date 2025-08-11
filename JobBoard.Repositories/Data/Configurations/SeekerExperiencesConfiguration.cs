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
    public class SeekerExperiencesConfiguration : IEntityTypeConfiguration<SeekerExperience>
    {
        public void Configure(EntityTypeBuilder<SeekerExperience> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.JobTitle).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.CompanyName).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.Location).HasMaxLength(200).IsRequired(false);
            builder.Property(e => e.Description).HasMaxLength(2000).IsRequired(false);
            builder.Property(e => e.StartDate).IsRequired(false);
            builder.Property(e => e.EndDate).IsRequired(false);

           
        }
    }
}
