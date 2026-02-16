namespace CodeNight.Application.DTOs;

public class UserStateDto
{
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
