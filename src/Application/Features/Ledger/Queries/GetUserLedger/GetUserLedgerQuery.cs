using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Ledger.Queries.GetUserLedger;

public record GetUserLedgerQuery(Guid UserId) : IRequest<List<LedgerEntryDto>>;
