using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Ledger.Queries.GetUserLedger;

public class GetUserLedgerQueryHandler : IRequestHandler<GetUserLedgerQuery, ApiResponse<List<LedgerEntryDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUserLedgerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<LedgerEntryDto>>> Handle(
        GetUserLedgerQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = Math.Clamp(request.Limit, 1, 100), Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var query = _context.PointsLedgerEntries
            .AsNoTracking()
            .Where(pl => pl.UserId == request.UserId);

        if (request.From.HasValue)
            query = query.Where(pl => DateOnly.FromDateTime(pl.CreatedAt) >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(pl => DateOnly.FromDateTime(pl.CreatedAt) <= request.To.Value);

        var entries = await query
            .OrderByDescending(pl => pl.CreatedAt)
            .Skip(offset)
            .Take(pagination.Limit)
            .Select(pl => new LedgerEntryDto
            {
                LedgerId = pl.LedgerId,
                UserId = pl.UserId,
                PointsDelta = pl.PointsDelta,
                Source = pl.Source,
                SourceRef = pl.SourceRef,
                CreatedAt = pl.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<LedgerEntryDto>>
        {
            Data = entries,
            Meta = new MetaInfo
            {
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, entries.Count)
            }
        };
    }
}
