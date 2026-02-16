using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserDetail;

public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, ApiResponse<UserDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<UserDetailDto>> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserState)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with id {request.UserId} not found.");

        var result = new UserDetailDto
        {
            User = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Surname = user.Surname,
                City = user.City,
                Role = user.Role.ToString(),
                TotalPoints = user.UserState?.TotalPoints ?? 0
            },
            State = user.UserState != null
                ? new UserStateDto
                {
                    UserId = user.UserId,
                    AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd"),
                    ListenMinutesToday = user.UserState.ListenMinutesToday,
                    UniqueTracksToday = user.UserState.UniqueTracksToday,
                    PlaylistAdditionsToday = user.UserState.PlaylistAdditionsToday,
                    SharesToday = user.UserState.SharesToday,
                    ListenMinutes7d = user.UserState.ListenMinutes7d,
                    Shares7d = user.UserState.Shares7d,
                    UniqueTracks7d = user.UserState.UniqueTracks7d,
                    ListenStreakDays = user.UserState.ListenStreakDays,
                    TotalPoints = user.UserState.TotalPoints
                }
                : null
        };

        return new ApiResponse<UserDetailDto>
        {
            Data = result,
            Meta = new MetaInfo { AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd") }
        };
    }
}
