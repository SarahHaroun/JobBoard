using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Repositories.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(500); // Assuming a maximum length for the message
            builder.Property(n => n.Link)
               .HasMaxLength(250);
            builder.Property(n => n.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(n => n.IsRead)
                   .HasDefaultValue(false);
            // Foreign key relationship with ApplicationUser
            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    

}

