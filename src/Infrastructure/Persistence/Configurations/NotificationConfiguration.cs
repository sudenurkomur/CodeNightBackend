using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.NotificationId);

        builder.Property(n => n.NotificationId)
            .HasColumnName("notification_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(n => n.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(n => n.Channel)
            .HasColumnName("channel")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.Message)
            .HasColumnName("message")
            .HasColumnType("varchar(2000)")
            .IsRequired();

        builder.Property(n => n.SentAt)
            .HasColumnName("sent_at")
            .IsRequired();

        // FK -> users
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(n => new { n.UserId, n.SentAt })
            .HasDatabaseName("ix_notifications_user_id_sent_at");
    }
}
