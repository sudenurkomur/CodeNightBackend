using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Event> Events { get; }
    DbSet<UserState> UserStates { get; }
    DbSet<Challenge> Challenges { get; }
    DbSet<ChallengeAward> ChallengeAwards { get; }
    DbSet<TriggeredChallenge> TriggeredChallenges { get; }
    DbSet<ChallengeDecision> ChallengeDecisions { get; }
    DbSet<PointsLedgerEntry> PointsLedgerEntries { get; }
    DbSet<Badge> Badges { get; }
    DbSet<BadgeAward> BadgeAwards { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Artist> Artists { get; }
    DbSet<Track> Tracks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
