using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.GrantUserRole;

public sealed record GrantUserRoleCommand(Guid ActorId, Guid UserId, string RoleName) : IRequest;
