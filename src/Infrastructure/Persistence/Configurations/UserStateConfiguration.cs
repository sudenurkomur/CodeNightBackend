using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class UserStateConfiguration : IEntityTypeConfiguration<UserState>
{
    public void Configure(EntityTypeBuilder<UserState> builder)
    {
        builder.ToTable("user_states");

        builder.HasKey(us => us.UserId);

        builder.Property(us => us.UserId)
            .HasColumnName("user_id");

        builder.Property(us => us.ListenMinutesToday)
            .HasColumnName("listen_minutes_today")
            .IsRequired();

        builder.Property(us => us.UniqueTracksToday)
            .HasColumnName("unique_tracks_today")
            .IsRequired();

        builder.Property(us => us.PlaylistAdditionsToday)
            .HasColumnName("playlist_additions_today")
            .IsRequired();

        builder.Property(us => us.SharesToday)
            .HasColumnName("shares_today")
            .IsRequired();

        builder.Property(us => us.ListenMinutes7d)
            .HasColumnName("listen_minutes_7d")
            .IsRequired();

        builder.Property(us => us.Shares7d)
            .HasColumnName("shares_7d")
            .IsRequired();

        builder.Property(us => us.UniqueTracks7d)
            .HasColumnName("unique_tracks_7d")
            .IsRequired();

        builder.Property(us => us.ListenStreakDays)
            .HasColumnName("listen_streak_days")
            .IsRequired();

        builder.Property(us => us.TotalPoints)
            .HasColumnName("total_points")
            .IsRequired();

        // 1-1 relationship with User
        builder.HasOne(us => us.User)
            .WithOne(u => u.UserState)
            .HasForeignKey<UserState>(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
