using CodeNight.Application.Features.Users.Queries.GetUserDashboard;
using CodeNight.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/dashboard")]
    public async Task<IActionResult> GetUserDashboard(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserDashboardQuery(id), cancellationToken);
        return Ok(result);
    }
}
