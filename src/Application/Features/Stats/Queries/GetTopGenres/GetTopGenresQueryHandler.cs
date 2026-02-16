using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Stats.Queries.GetTopGenres;

public class GetTopGenresQueryHandler : IRequestHandler<GetTopGenresQuery, ApiResponse<List<TopGenreDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetTopGenresQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<TopGenreDto>>> Handle(
        GetTopGenresQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Events.AsNoTracking().AsQueryable();

        if (request.Window.Equals("7d", StringComparison.OrdinalIgnoreCase))
        {
            var from = request.AsOfDate.AddDays(-6);
            query = query.Where(e => e.Date >= from && e.Date <= request.AsOfDate);
        }
        else
        {
            // default: today
            query = query.Where(e => e.Date == request.AsOfDate);
        }

        var genres = await query
            .GroupBy(e => e.TopGenre)
            .Select(g => new TopGenreDto
            {
                Genre = g.Key.ToString(),
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<TopGenreDto>>
        {
            Data = genres,
            Meta = new MetaInfo
            {
                AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd"),
                Window = request.Window
            }
        };
    }
}
