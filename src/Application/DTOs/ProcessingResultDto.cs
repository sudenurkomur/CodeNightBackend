namespace CodeNight.Application.DTOs;

public class ProcessingResultDto
{
    public int UsersProcessed { get; set; }
    public int ChallengesTriggered { get; set; }
    public int AwardsGiven { get; set; }
    public int BadgesAwarded { get; set; }
    public int NotificationsSent { get; set; }
    public DateOnly AsOfDate { get; set; }
}
