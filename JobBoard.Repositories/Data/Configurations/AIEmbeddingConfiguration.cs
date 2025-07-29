using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Repositories.Data.Configurations
{
    public class AIEmbeddingConfiguration : IEntityTypeConfiguration<AIEmbedding>
    {
        public void Configure(EntityTypeBuilder<AIEmbedding> builder)
        {
            builder.HasKey(e => e.Id);

            // save embedding Vector string (comma-separated)
            builder.Property(e => e.EmbeddingVector)
                   .HasConversion(
                       v => string.Join(',', v),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(float.Parse).ToArray()
                   );

            builder.Property(e => e.EntityType).IsRequired();
            builder.Property(e => e.Content).IsRequired();
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
