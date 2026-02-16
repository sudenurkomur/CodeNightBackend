using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler
    : IRequestHandler<GetUserNotificationsQuery, ApiResponse<List<NotificationDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUserNotificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<NotificationDto>>> Handle(
        GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = Math.Clamp(request.Limit, 1, 100), Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var query = _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == request.UserId);

        if (request.From.HasValue)
            query = query.Where(n => DateOnly.FromDateTime(n.SentAt) >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(n => DateOnly.FromDateTime(n.SentAt) <= request.To.Value);

        var notifications = await query
            .OrderByDescending(n => n.SentAt)
            .Skip(offset)
            .Take(pagination.Limit)
            .Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                UserId = n.UserId,
                Channel = n.Channel.ToString(),
                Message = n.Message,
                SentAt = n.SentAt
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<NotificationDto>>
        {
            Data = notifications,
            Meta = new MetaInfo
            {
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, notifications.Count)
            }
        };
    }
}
