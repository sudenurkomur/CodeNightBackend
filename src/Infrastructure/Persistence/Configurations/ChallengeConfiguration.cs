using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class ChallengeConfiguration : IEntityTypeConfiguration<Challenge>
{
    public void Configure(EntityTypeBuilder<Challenge> builder)
    {
        builder.ToTable("challenges");

        builder.HasKey(c => c.ChallengeId);

        builder.Property(c => c.ChallengeId)
            .HasColumnName("challenge_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.ChallengeName)
            .HasColumnName("challenge_name")
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(c => c.ChallengeType)
            .HasColumnName("challenge_type")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.Condition)
            .HasColumnName("condition")
            .HasColumnType("varchar(1000)")
            .IsRequired();

        builder.Property(c => c.RewardPoints)
            .HasColumnName("reward_points")
            .IsRequired();

        builder.Property(c => c.Priority)
            .HasColumnName("priority")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}
