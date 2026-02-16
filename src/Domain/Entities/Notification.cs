using CodeNight.Domain.Enums;

namespace CodeNight.Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public NotificationChannel Channel { get; set; }
    public string Message { get; set; } = null!;
    public DateTime SentAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
