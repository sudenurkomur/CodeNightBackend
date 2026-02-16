using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class BadgeAwardConfiguration : IEntityTypeConfiguration<BadgeAward>
{
    public void Configure(EntityTypeBuilder<BadgeAward> builder)
    {
        builder.ToTable("badge_awards");

        // Composite PK
        builder.HasKey(ba => new { ba.UserId, ba.BadgeId });

        builder.Property(ba => ba.UserId)
            .HasColumnName("user_id");

        builder.Property(ba => ba.BadgeId)
            .HasColumnName("badge_id");

        builder.Property(ba => ba.AwardedAt)
            .HasColumnName("awarded_at")
            .IsRequired();

        // FK -> users
        builder.HasOne(ba => ba.User)
            .WithMany(u => u.BadgeAwards)
            .HasForeignKey(ba => ba.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK -> badges
        builder.HasOne(ba => ba.Badge)
            .WithMany(b => b.BadgeAwards)
            .HasForeignKey(ba => ba.BadgeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
