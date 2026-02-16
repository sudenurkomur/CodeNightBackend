using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUserDashboard;

public record GetUserDashboardQuery(Guid UserId) : IRequest<UserDashboardDto>;
