namespace CodeNight.Domain.Entities;

public class BadgeAward
{
    public Guid UserId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime AwardedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}
