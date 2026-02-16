namespace CodeNight.Domain.Entities;

public class UserState
{
    public Guid UserId { get; set; }
    public long ListenMinutesToday { get; set; }
    public long UniqueTracksToday { get; set; }
    public long PlaylistAdditionsToday { get; set; }
    public long SharesToday { get; set; }
    public long ListenMinutes7d { get; set; }
    public long Shares7d { get; set; }
    public long UniqueTracks7d { get; set; }
    public long ListenStreakDays { get; set; }
    public long TotalPoints { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
