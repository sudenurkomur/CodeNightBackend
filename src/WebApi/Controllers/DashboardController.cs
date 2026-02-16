using CodeNight.Application.Features.Dashboard.Queries.GetDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/dashboard — Hazır paket: users preview + leaderboard top10 + top genre
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetDashboard(
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate)
    {
        var result = await _mediator.Send(new GetDashboardQuery(asOfDate));
        return Ok(result);
    }
}
