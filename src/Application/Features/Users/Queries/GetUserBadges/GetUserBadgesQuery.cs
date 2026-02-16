using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUserBadges;

public record GetUserBadgesQuery(Guid UserId, DateOnly AsOfDate) : IRequest<ApiResponse<UserBadgesDto>>;
