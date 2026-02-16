using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(u => u.UserState)
            .AsNoTracking()
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Surname = u.Surname,
                City = u.City,
                Role = u.Role.ToString(),
                TotalPoints = u.UserState != null ? u.UserState.TotalPoints : 0
            })
            .ToListAsync(cancellationToken);

        return users;
    }
}
