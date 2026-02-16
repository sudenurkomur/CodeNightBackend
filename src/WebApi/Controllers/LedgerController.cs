using CodeNight.Application.Features.Ledger.Queries.GetUserLedger;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class LedgerController : ControllerBase
{
    private readonly IMediator _mediator;

    public LedgerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/ledger")]
    public async Task<IActionResult> GetUserLedger(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserLedgerQuery(id), cancellationToken);
        return Ok(result);
    }
}
