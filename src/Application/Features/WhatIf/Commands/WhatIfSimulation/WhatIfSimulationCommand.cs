using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.WhatIf.Commands.WhatIfSimulation;

public record WhatIfSimulationCommand(
    Guid UserId,
    DateOnly AsOfDate,
    Dictionary<string, long> Delta
) : IRequest<ApiResponse<WhatIfResultDto>>;
