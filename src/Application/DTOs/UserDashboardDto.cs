namespace CodeNight.Application.DTOs;

public class UserDashboardDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public UserStateDto? UserState { get; set; }
    public ChallengeAwardDto? LatestAward { get; set; }
    public List<BadgeDto> Badges { get; set; } = new();
    public List<NotificationDto> Notifications { get; set; } = new();
}
