using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("artists");

        builder.HasKey(a => a.ArtistId);

        builder.Property(a => a.ArtistId)
            .HasColumnName("artist_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(a => a.ArtistName)
            .HasColumnName("artist_name")
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(a => a.Genre)
            .HasColumnName("genre")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();
    }
}
