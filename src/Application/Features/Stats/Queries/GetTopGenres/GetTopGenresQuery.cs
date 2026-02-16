using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Stats.Queries.GetTopGenres;

public record GetTopGenresQuery(
    DateOnly AsOfDate,
    string Window = "today",
    int Limit = 10
) : IRequest<ApiResponse<List<TopGenreDto>>>;
