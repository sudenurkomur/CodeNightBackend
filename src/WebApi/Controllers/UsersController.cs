using CodeNight.Application.Features.Users.Queries.GetUsers;
using CodeNight.Application.Features.Users.Queries.GetUserDetail;
using CodeNight.Application.Features.Users.Queries.GetUserChallengeAwards;
using CodeNight.Application.Features.Users.Queries.GetUserBadges;
using CodeNight.Application.Features.Users.Queries.GetUserNotifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/users — Kullanıcı listesi + total_points
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate,
        [FromQuery(Name = "q")] string? search = null,
        [FromQuery] int limit = 25,
        [FromQuery] string? cursor = null,
        [FromQuery] string? sort = null)
    {
        var result = await _mediator.Send(new GetUsersQuery(asOfDate, search, limit, cursor, sort));
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/users/{userId} — User + state detay
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserDetail(
        Guid userId,
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate)
    {
        var result = await _mediator.Send(new GetUserDetailQuery(userId, asOfDate));
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/users/{userId}/challenge-awards — Triggered / Selected / Suppressed
    /// </summary>
    [HttpGet("{userId:guid}/challenge-awards")]
    public async Task<IActionResult> GetUserChallengeAwards(
        Guid userId,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        [FromQuery] int limit = 30,
        [FromQuery] string? cursor = null)
    {
        var result = await _mediator.Send(new GetUserChallengeAwardsQuery(userId, from, to, limit, cursor));
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/users/{userId}/badges — Kullanıcının kazandığı rozetler
    /// </summary>
    [HttpGet("{userId:guid}/badges")]
    public async Task<IActionResult> GetUserBadges(
        Guid userId,
        [FromQuery(Name = "as_of_date")] DateOnly asOfDate)
    {
        var result = await _mediator.Send(new GetUserBadgesQuery(userId, asOfDate));
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/users/{userId}/notifications — Bildirim kayıtları
    /// </summary>
    [HttpGet("{userId:guid}/notifications")]
    public async Task<IActionResult> GetUserNotifications(
        Guid userId,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        [FromQuery] int limit = 50,
        [FromQuery] string? cursor = null)
    {
        var result = await _mediator.Send(new GetUserNotificationsQuery(userId, from, to, limit, cursor));
        return Ok(result);
    }

    /// <summary>
    /// GET /api/v1/users/{userId}/points-ledger — Puan grafiği ve audit
    /// </summary>
    [HttpGet("{userId:guid}/points-ledger")]
    public async Task<IActionResult> GetUserLedger(
        Guid userId,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null,
        [FromQuery] int limit = 50,
        [FromQuery] string? cursor = null)
    {
        var result = await _mediator.Send(
            new CodeNight.Application.Features.Ledger.Queries.GetUserLedger.GetUserLedgerQuery(
                userId, from, to, limit, cursor));
        return Ok(result);
    }
}
