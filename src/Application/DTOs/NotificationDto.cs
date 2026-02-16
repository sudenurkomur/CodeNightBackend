namespace CodeNight.Application.DTOs;

public class NotificationDto
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public string Channel { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime SentAt { get; set; }
}
