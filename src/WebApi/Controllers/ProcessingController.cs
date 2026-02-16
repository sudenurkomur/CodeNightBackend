using CodeNight.Application.Features.Processing.Commands.RunProcessing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("run")]
    public async Task<IActionResult> RunProcessing(
        [FromQuery] DateOnly asOfDate,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RunProcessingCommand(asOfDate), cancellationToken);
        return Ok(result);
    }
}
