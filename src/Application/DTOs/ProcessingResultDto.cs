namespace CodeNight.Application.DTOs;

public class ProcessingResultDto
{
    public DateOnly AsOfDate { get; set; }
    public int UsersProcessed { get; set; }
    public int ChallengesTriggered { get; set; }
    public int AwardsGiven { get; set; }
    public int BadgesAwarded { get; set; }
    public int NotificationsSent { get; set; }
}
