using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<List<UserDto>>;
