using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string City { get; set; } = null!;
    public Role Role { get; set; }

    // Navigation properties
    public UserState? UserState { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<ChallengeAward> ChallengeAwards { get; set; } = new List<ChallengeAward>();
    public ICollection<PointsLedgerEntry> PointsLedgerEntries { get; set; } = new List<PointsLedgerEntry>();
    public ICollection<BadgeAward> BadgeAwards { get; set; } = new List<BadgeAward>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
