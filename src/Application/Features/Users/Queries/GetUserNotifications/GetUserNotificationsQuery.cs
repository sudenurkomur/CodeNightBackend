using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUserNotifications;

public record GetUserNotificationsQuery(
    Guid UserId,
    DateOnly? From = null,
    DateOnly? To = null,
    int Limit = 50,
    string? Cursor = null
) : IRequest<ApiResponse<List<NotificationDto>>>;
