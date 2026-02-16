using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class ChallengeAwardConfiguration : IEntityTypeConfiguration<ChallengeAward>
{
    public void Configure(EntityTypeBuilder<ChallengeAward> builder)
    {
        builder.ToTable("challenge_awards");

        builder.HasKey(ca => ca.AwardId);

        builder.Property(ca => ca.AwardId)
            .HasColumnName("award_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(ca => ca.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(ca => ca.DecisionId)
            .HasColumnName("decision_id")
            .IsRequired();

        builder.Property(ca => ca.AsOfDate)
            .HasColumnName("as_of_date")
            .IsRequired();

        builder.Property(ca => ca.RewardPoints)
            .HasColumnName("reward_points")
            .IsRequired();

        builder.Property(ca => ca.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        // FK -> users
        builder.HasOne(ca => ca.User)
            .WithMany(u => u.ChallengeAwards)
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK -> challenge_decisions
        builder.HasOne(ca => ca.ChallengeDecision)
            .WithMany(cd => cd.ChallengeAwards)
            .HasForeignKey(ca => ca.DecisionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for idempotency
        builder.HasIndex(ca => new { ca.UserId, ca.AsOfDate })
            .HasDatabaseName("ix_challenge_awards_user_id_as_of_date")
            .IsUnique();
    }
}
