using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApiResponse<List<UserDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = Math.Clamp(request.Limit, 1, 100), Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var query = _context.Users
            .Include(u => u.UserState)
            .AsNoTracking()
            .AsQueryable();

        // Search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(u =>
                u.Name.ToLower().Contains(search) ||
                u.Surname.ToLower().Contains(search) ||
                u.City.ToLower().Contains(search));
        }

        // Sorting
        query = request.Sort?.ToLowerInvariant() switch
        {
            "points_asc" => query.OrderBy(u => u.UserState != null ? u.UserState.TotalPoints : 0),
            "name_asc" => query.OrderBy(u => u.Name),
            _ => query.OrderByDescending(u => u.UserState != null ? u.UserState.TotalPoints : 0) // default: points_desc
        };

        var users = await query
            .Skip(offset)
            .Take(pagination.Limit)
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

        return new ApiResponse<List<UserDto>>
        {
            Data = users,
            Meta = new MetaInfo
            {
                AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd"),
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, users.Count)
            }
        };
    }
}
