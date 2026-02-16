using CodeNight.Application.Features.Processing.Commands.RunProcessing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/processing")]
public class ProcessingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// POST /api/v1/processing/run?asOfDate=YYYY-MM-DD — İdempotent işlem zinciri
    /// </summary>
    [HttpPost("run")]
    public async Task<IActionResult> RunProcessing([FromQuery] DateOnly asOfDate)
    {
        var result = await _mediator.Send(new RunProcessingCommand(asOfDate));
        return Ok(result);
    }
}
