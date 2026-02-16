using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserBadges;

public class GetUserBadgesQueryHandler : IRequestHandler<GetUserBadgesQuery, ApiResponse<UserBadgesDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserBadgesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<UserBadgesDto>> Handle(GetUserBadgesQuery request, CancellationToken cancellationToken)
    {
        var availableBadges = await _context.Badges
            .AsNoTracking()
            .Select(b => new BadgeDto
            {
                BadgeId = b.BadgeId,
                BadgeName = b.BadgeName,
                ThresholdPoints = b.Condition
            })
            .ToListAsync(cancellationToken);

        var awardedBadges = await _context.BadgeAwards
            .AsNoTracking()
            .Where(ba => ba.UserId == request.UserId)
            .Select(ba => new BadgeAwardDto
            {
                UserId = ba.UserId,
                BadgeId = ba.BadgeId,
                AwardedAt = ba.AwardedAt
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<UserBadgesDto>
        {
            Data = new UserBadgesDto
            {
                AvailableBadges = availableBadges,
                AwardedBadges = awardedBadges
            },
            Meta = new MetaInfo { AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd") }
        };
    }
}
