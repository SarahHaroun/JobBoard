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
	class SkillConfiguration : IEntityTypeConfiguration<Skill>
	{
		public void Configure(EntityTypeBuilder<Skill> builder)
		{
            builder.HasKey(s => s.Id);

            builder.Property(c => c.SkillName)
				.IsRequired().HasMaxLength(100); ;

			builder.HasIndex(c => c.SkillName)
				.IsUnique();
		}
	}
}
