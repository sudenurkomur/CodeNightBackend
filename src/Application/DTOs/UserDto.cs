namespace CodeNight.Application.DTOs;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Role { get; set; } = null!;
    public long TotalPoints { get; set; }
}
