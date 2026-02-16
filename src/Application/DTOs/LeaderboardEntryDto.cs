namespace CodeNight.Application.DTOs;

public class LeaderboardEntryDto
{
    public long Rank { get; set; }
    public Guid UserId { get; set; }
    public long TotalPoints { get; set; }
}
