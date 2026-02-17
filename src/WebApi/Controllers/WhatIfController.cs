using CodeNight.Application.Features.WhatIf.Commands.WhatIfSimulation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/what-if")]
public class WhatIfController : ControllerBase
{
    private readonly IMediator _mediator;

    public WhatIfController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// POST /api/v1/what-if/users/{userId} — What-if simülasyonu
    /// </summary>
    [HttpPost("users/{userId:guid}")]
    public async Task<IActionResult> WhatIfSimulation(
        Guid userId,
        [FromBody] WhatIfRequest body)
    {
        if (!body.AsOfDate.HasValue)
            return BadRequest(new { error = new { code = "VALIDATION_ERROR", message = "as_of_date is required" } });

        var command = new WhatIfSimulationCommand(userId, body.AsOfDate.Value, body.Delta);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

public class WhatIfRequest
{
    public DateOnly? AsOfDate { get; set; }
    public Dictionary<string, long> Delta { get; set; } = new();
}
