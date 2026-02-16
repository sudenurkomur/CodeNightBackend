using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(
    DateOnly AsOfDate,
    string? Search = null,
    int Limit = 25,
    string? Cursor = null,
    string? Sort = null
) : IRequest<ApiResponse<List<UserDto>>>;
