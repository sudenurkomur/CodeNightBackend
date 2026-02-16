using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUserDetail;

public record GetUserDetailQuery(Guid UserId, DateOnly AsOfDate) : IRequest<ApiResponse<UserDetailDto>>;
