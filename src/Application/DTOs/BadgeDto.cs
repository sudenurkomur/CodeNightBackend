namespace CodeNight.Application.DTOs;

public class BadgeDto
{
    public Guid BadgeId { get; set; }
    public string BadgeName { get; set; } = null!;
    public long ThresholdPoints { get; set; }
}

public class BadgeAwardDto
{
    public Guid UserId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime AwardedAt { get; set; }
}

public class UserBadgesDto
{
    public List<BadgeDto> AvailableBadges { get; set; } = new();
    public List<BadgeAwardDto> AwardedBadges { get; set; } = new();
}
