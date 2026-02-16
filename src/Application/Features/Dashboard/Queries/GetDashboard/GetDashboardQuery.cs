using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery(DateOnly AsOfDate) : IRequest<ApiResponse<DashboardDto>>;
