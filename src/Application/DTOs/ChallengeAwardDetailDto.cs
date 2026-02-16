namespace CodeNight.Application.DTOs;

public class ChallengeAwardDetailDto
{
    public Guid AwardId { get; set; }
    public Guid UserId { get; set; }
    public string AsOfDate { get; set; } = null!;
    public List<Guid> TriggeredChallenges { get; set; } = new();
    public Guid SelectedChallenge { get; set; }
    public List<Guid> SuppressedChallenges { get; set; } = new();
    public long RewardPoints { get; set; }
    public DateTime Timestamp { get; set; }
}
