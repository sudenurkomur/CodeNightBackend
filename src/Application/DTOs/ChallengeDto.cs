namespace CodeNight.Application.DTOs;

public class ChallengeDto
{
    public Guid ChallengeId { get; set; }
    public string ChallengeName { get; set; } = null!;
    public string ChallengeType { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public long RewardPoints { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }
}
