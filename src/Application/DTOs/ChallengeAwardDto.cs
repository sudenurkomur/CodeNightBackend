namespace CodeNight.Application.DTOs;

public class ChallengeAwardDto
{
    public Guid AwardId { get; set; }
    public DateOnly AsOfDate { get; set; }
    public long RewardPoints { get; set; }
    public string ChallengeName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
