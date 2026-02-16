namespace CodeNight.Domain.Entities;

public class ChallengeAward
{
    public Guid AwardId { get; set; }
    public Guid UserId { get; set; }
    public Guid DecisionId { get; set; }
    public DateOnly AsOfDate { get; set; }
    public long RewardPoints { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ChallengeDecision ChallengeDecision { get; set; } = null!;
    public ICollection<TriggeredChallenge> TriggeredChallenges { get; set; } = new List<TriggeredChallenge>();
}
