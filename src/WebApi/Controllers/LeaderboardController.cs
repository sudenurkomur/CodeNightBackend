using CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaderboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaderboard(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLeaderboardQuery(), cancellationToken);
        return Ok(result);
    }
}
