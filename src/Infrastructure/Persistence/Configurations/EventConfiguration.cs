using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder.HasKey(e => e.EventId);

        builder.Property(e => e.EventId)
            .HasColumnName("event_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(e => e.ListenMinutes)
            .HasColumnName("listen_minutes")
            .IsRequired();

        builder.Property(e => e.UniqueTracks)
            .HasColumnName("unique_tracks")
            .IsRequired();

        builder.Property(e => e.PlaylistAdditions)
            .HasColumnName("playlist_additions")
            .IsRequired();

        builder.Property(e => e.Shares)
            .HasColumnName("shares")
            .IsRequired();

        builder.Property(e => e.TopGenre)
            .HasColumnName("top_genre")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();

        // FK
        builder.HasOne(e => e.User)
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(e => new { e.UserId, e.Date })
            .HasDatabaseName("ix_events_user_id_date");
    }
}
