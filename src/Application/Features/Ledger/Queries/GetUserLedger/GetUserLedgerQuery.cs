using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Ledger.Queries.GetUserLedger;

public record GetUserLedgerQuery(
    Guid UserId,
    DateOnly? From = null,
    DateOnly? To = null,
    int Limit = 50,
    string? Cursor = null
) : IRequest<ApiResponse<List<LedgerEntryDto>>>;
