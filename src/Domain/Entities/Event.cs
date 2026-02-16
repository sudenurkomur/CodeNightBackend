using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class Event
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public long ListenMinutes { get; set; }
    public long UniqueTracks { get; set; }
    public long PlaylistAdditions { get; set; }
    public long Shares { get; set; }
    public Genre TopGenre { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
