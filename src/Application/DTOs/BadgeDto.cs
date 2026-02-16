namespace CodeNight.Application.DTOs;

public class BadgeDto
{
    public Guid BadgeId { get; set; }
    public string BadgeName { get; set; } = null!;
    public string Level { get; set; } = null!;
    public DateTime AwardedAt { get; set; }
}
