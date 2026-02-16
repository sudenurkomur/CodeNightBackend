namespace CodeNight.Application.DTOs;

public class LeaderboardEntryDto
{
    public long Rank { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string City { get; set; } = null!;
    public long TotalPoints { get; set; }
}
