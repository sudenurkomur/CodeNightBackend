using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class PointsLedgerConfiguration : IEntityTypeConfiguration<PointsLedgerEntry>
{
    public void Configure(EntityTypeBuilder<PointsLedgerEntry> builder)
    {
        builder.ToTable("points_ledger");

        builder.HasKey(pl => pl.LedgerId);

        builder.Property(pl => pl.LedgerId)
            .HasColumnName("ledger_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(pl => pl.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(pl => pl.PointsDelta)
            .HasColumnName("points_delta")
            .IsRequired();

        builder.Property(pl => pl.Source)
            .HasColumnName("source")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(pl => pl.SourceRef)
            .HasColumnName("source_ref")
            .IsRequired();

        builder.Property(pl => pl.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // FK -> users
        builder.HasOne(pl => pl.User)
            .WithMany(u => u.PointsLedgerEntries)
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(pl => new { pl.UserId, pl.CreatedAt })
            .HasDatabaseName("ix_points_ledger_user_id_created_at");

        // Idempotency unique index
        builder.HasIndex(pl => new { pl.Source, pl.SourceRef })
            .HasDatabaseName("ix_points_ledger_source_source_ref")
            .IsUnique();
    }
}
