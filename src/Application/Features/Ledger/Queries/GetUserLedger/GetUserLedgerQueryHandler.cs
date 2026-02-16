using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Ledger.Queries.GetUserLedger;

public class GetUserLedgerQueryHandler : IRequestHandler<GetUserLedgerQuery, List<LedgerEntryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserLedgerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LedgerEntryDto>> Handle(GetUserLedgerQuery request, CancellationToken cancellationToken)
    {
        var entries = await _context.PointsLedgerEntries
            .AsNoTracking()
            .Where(pl => pl.UserId == request.UserId)
            .OrderBy(pl => pl.CreatedAt)
            .Select(pl => new LedgerEntryDto
            {
                Date = DateOnly.FromDateTime(pl.CreatedAt),
                PointsDelta = pl.PointsDelta,
                Source = pl.Source,
                RunningTotal = 0 // will be computed below
            })
            .ToListAsync(cancellationToken);

        // Compute running total
        long runningTotal = 0;
        foreach (var entry in entries)
        {
            runningTotal += entry.PointsDelta;
            entry.RunningTotal = runningTotal;
        }

        return entries;
    }
}
