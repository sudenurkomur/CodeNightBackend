using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class TriggeredChallengeConfiguration : IEntityTypeConfiguration<TriggeredChallenge>
{
    public void Configure(EntityTypeBuilder<TriggeredChallenge> builder)
    {
        builder.ToTable("triggered_challenges");

        // Composite PK
        builder.HasKey(tc => new { tc.AwardId, tc.ChallengeId });

        builder.Property(tc => tc.AwardId)
            .HasColumnName("award_id");

        builder.Property(tc => tc.ChallengeId)
            .HasColumnName("challenge_id");

        builder.Property(tc => tc.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(50)")
            .HasConversion<string>()
            .IsRequired();

        // FK -> challenge_awards
        builder.HasOne(tc => tc.ChallengeAward)
            .WithMany(ca => ca.TriggeredChallenges)
            .HasForeignKey(tc => tc.AwardId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK -> challenges
        builder.HasOne(tc => tc.Challenge)
            .WithMany(c => c.TriggeredChallenges)
            .HasForeignKey(tc => tc.ChallengeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index
        builder.HasIndex(tc => tc.ChallengeId)
            .HasDatabaseName("ix_triggered_challenges_challenge_id");
    }
}
