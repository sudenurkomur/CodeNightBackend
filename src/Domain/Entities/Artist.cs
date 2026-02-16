using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class Artist
{
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = null!;
    public Genre Genre { get; set; }

    // Navigation
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
