namespace CodeNight.Application.DTOs;

public class DashboardDto
{
    public List<UserDto> UsersPreview { get; set; } = new();
    public List<LeaderboardEntryDto> LeaderboardTop10 { get; set; } = new();
    public List<TopGenreDto> TopGenreDistribution { get; set; } = new();
}
