using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class Badge
{
    public Guid BadgeId { get; set; }
    public string BadgeName { get; set; } = null!;
    public long Condition { get; set; }
    public BadgeLevel Level { get; set; }

    // Navigation
    public ICollection<BadgeAward> BadgeAwards { get; set; } = new List<BadgeAward>();
}
