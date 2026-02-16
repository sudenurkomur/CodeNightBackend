using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class Challenge
{
    public Guid ChallengeId { get; set; }
    public string ChallengeName { get; set; } = null!;
    public ChallengeType ChallengeType { get; set; }
    public string Condition { get; set; } = null!;
    public long RewardPoints { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<TriggeredChallenge> TriggeredChallenges { get; set; } = new List<TriggeredChallenge>();
}
