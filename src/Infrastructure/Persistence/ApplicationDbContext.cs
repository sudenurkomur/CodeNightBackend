using System.Reflection;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<UserState> UserStates => Set<UserState>();
    public DbSet<Challenge> Challenges => Set<Challenge>();
    public DbSet<ChallengeAward> ChallengeAwards => Set<ChallengeAward>();
    public DbSet<TriggeredChallenge> TriggeredChallenges => Set<TriggeredChallenge>();
    public DbSet<ChallengeDecision> ChallengeDecisions => Set<ChallengeDecision>();
    public DbSet<PointsLedgerEntry> PointsLedgerEntries => Set<PointsLedgerEntry>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<BadgeAward> BadgeAwards => Set<BadgeAward>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Track> Tracks => Set<Track>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
