namespace CodeNight.Domain.Entities;

public class ChallengeDecision
{
    public Guid DecisionId { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<ChallengeAward> ChallengeAwards { get; set; } = new List<ChallengeAward>();
}
