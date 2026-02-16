using CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaderboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/leaderboard — Top N kullanıcı (default 10)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard(
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate,
        [FromQuery] int limit = 10,
        [FromQuery] string? cursor = null)
    {
        var result = await _mediator.Send(new GetLeaderboardQuery(asOfDate, limit, cursor));
        return Ok(result);
    }
}
