using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.ToTable("badges");

        builder.HasKey(b => b.BadgeId);

        builder.Property(b => b.BadgeId)
            .HasColumnName("badge_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(b => b.BadgeName)
            .HasColumnName("badge_name")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(b => b.Condition)
            .HasColumnName("condition")
            .IsRequired();

        builder.Property(b => b.Level)
            .HasColumnName("level")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();
    }
}
