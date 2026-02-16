using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserBadges;

public class GetUserBadgesQueryHandler
    : IRequestHandler<GetUserBadgesQuery, ApiResponse<UserBadgesDto>>
{
    private readonly IApplicationDbContext _db;

    public GetUserBadgesQueryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<UserBadgesDto>> Handle(
        GetUserBadgesQuery request, CancellationToken cancellationToken)
    {
        var allBadges = await _db.Badges
            .OrderBy(b => b.Condition)
            .ToListAsync(cancellationToken);

        var awardedBadges = await _db.BadgeAwards
            .Where(ba => ba.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var dto = new UserBadgesDto
        {
            AvailableBadges = allBadges.Select(b => new BadgeDto
            {
                BadgeId = b.BadgeId,
                BadgeName = b.BadgeName,
                ThresholdPoints = b.Condition
            }).ToList(),
            AwardedBadges = awardedBadges.Select(ba => new BadgeAwardDto
            {
                UserId = ba.UserId,
                BadgeId = ba.BadgeId,
                AwardedAt = ba.AwardedAt
            }).ToList()
        };

        return new ApiResponse<UserBadgesDto>
        {
            Data = dto,
            Meta = new MetaInfo { AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd") }
        };
    }
}
