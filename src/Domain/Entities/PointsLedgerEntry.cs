namespace CodeNight.Domain.Entities;

public class PointsLedgerEntry
{
    public Guid LedgerId { get; set; }
    public Guid UserId { get; set; }
    public long PointsDelta { get; set; }
    public string Source { get; set; } = null!;
    public Guid SourceRef { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
