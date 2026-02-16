namespace CodeNight.Application.DTOs;

public class LedgerEntryDto
{
    public DateOnly Date { get; set; }
    public long PointsDelta { get; set; }
    public long RunningTotal { get; set; }
    public string Source { get; set; } = null!;
}
