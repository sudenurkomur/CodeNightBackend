using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Processing.Commands.RunProcessing;

public record RunProcessingCommand(DateOnly AsOfDate) : IRequest<ApiResponse<ProcessingResultDto>>;
