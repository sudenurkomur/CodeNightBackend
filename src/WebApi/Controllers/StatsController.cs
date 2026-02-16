using CodeNight.Application.Features.Stats.Queries.GetTopGenres;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/stats")]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/stats/top-genres — Top genre dağılımı
    /// </summary>
    [HttpGet("top-genres")]
    public async Task<IActionResult> GetTopGenres(
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate,
        [FromQuery] string window = "today",
        [FromQuery] int limit = 10)
    {
        var result = await _mediator.Send(new GetTopGenresQuery(asOfDate, window, limit));
        return Ok(result);
    }
}
