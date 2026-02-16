namespace CodeNight.Application.DTOs;

public class UserStateDto
{
    public Guid UserId { get; set; }
    public string AsOfDate { get; set; } = null!;
    public long ListenMinutesToday { get; set; }
    public long UniqueTracksToday { get; set; }
    public long PlaylistAdditionsToday { get; set; }
    public long SharesToday { get; set; }
    public long ListenMinutes7d { get; set; }
    public long Shares7d { get; set; }
    public long UniqueTracks7d { get; set; }
    public long ListenStreakDays { get; set; }
    public long TotalPoints { get; set; }
}
