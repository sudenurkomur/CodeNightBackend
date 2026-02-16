using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class TriggeredChallenge
{
    public Guid AwardId { get; set; }
    public Guid ChallengeId { get; set; }
    public TriggeredChallengeStatus Status { get; set; }

    // Navigation
    public ChallengeAward ChallengeAward { get; set; } = null!;
    public Challenge Challenge { get; set; } = null!;
}
