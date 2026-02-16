using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("tracks");

        builder.HasKey(t => t.TrackId);

        builder.Property(t => t.TrackId)
            .HasColumnName("track_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(t => t.ArtistId)
            .HasColumnName("artist_id")
            .IsRequired();

        builder.Property(t => t.TrackName)
            .HasColumnName("track_name")
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(t => t.DurationSec)
            .HasColumnName("duration_sec")
            .IsRequired();

        // FK -> artists
        builder.HasOne(t => t.Artist)
            .WithMany(a => a.Tracks)
            .HasForeignKey(t => t.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
