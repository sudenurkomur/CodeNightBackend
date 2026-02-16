using CodeNight.Application.Features.Challenges.Commands.CreateChallenge;
using CodeNight.Application.Features.Challenges.Commands.UpdateChallenge;
using CodeNight.Application.Features.Challenges.Queries.GetChallenges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNight.WebApi.Controllers;

[ApiController]
[Route("api/v1/challenges")]
public class ChallengesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChallengesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// GET /api/v1/challenges — Challenge listesi
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetChallenges(
        [FromQuery(Name = "is_active")] bool? isActive = null,
        [FromQuery] string? type = null,
        [FromQuery] string? sort = null)
    {
        var result = await _mediator.Send(new GetChallengesQuery(isActive, type, sort));
        return Ok(result);
    }

    /// <summary>
    /// POST /api/v1/challenges — Yeni challenge oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateChallenge([FromBody] CreateChallengeCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// PATCH /api/v1/challenges/{challengeId} — Challenge güncelle (partial)
    /// </summary>
    [HttpPatch("{challengeId:guid}")]
    public async Task<IActionResult> UpdateChallenge(
        Guid challengeId,
        [FromBody] UpdateChallengeRequest body)
    {
        var command = new UpdateChallengeCommand(
            challengeId,
            body.ChallengeName,
            body.ChallengeType,
            body.Condition,
            body.RewardPoints,
            body.Priority,
            body.IsActive);

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

/// <summary>
/// PATCH request body for partial challenge update
/// </summary>
public class UpdateChallengeRequest
{
    public string? ChallengeName { get; set; }
    public string? ChallengeType { get; set; }
    public string? Condition { get; set; }
    public long? RewardPoints { get; set; }
    public int? Priority { get; set; }
    public bool? IsActive { get; set; }
}
