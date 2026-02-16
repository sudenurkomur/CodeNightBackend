using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeNight.Infrastructure.Persistence.Configurations;

public class ChallengeDecisionConfiguration : IEntityTypeConfiguration<ChallengeDecision>
{
    public void Configure(EntityTypeBuilder<ChallengeDecision> builder)
    {
        builder.ToTable("challenge_decisions");

        builder.HasKey(cd => cd.DecisionId);

        builder.Property(cd => cd.DecisionId)
            .HasColumnName("decision_id")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(cd => cd.Reason)
            .HasColumnName("reason")
            .HasColumnType("varchar(1000)")
            .IsRequired();

        builder.Property(cd => cd.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}
