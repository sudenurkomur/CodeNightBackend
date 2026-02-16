namespace CodeNight.Domain.Entities;

public class Track
{
    public Guid TrackId { get; set; }
    public Guid ArtistId { get; set; }
    public string TrackName { get; set; } = null!;
    public long DurationSec { get; set; }

    // Navigation
    public Artist Artist { get; set; } = null!;
}
