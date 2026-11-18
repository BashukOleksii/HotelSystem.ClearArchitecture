using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.RevokeUserRole;

public sealed record RevokeUserRoleCommand(Guid ActorId, Guid UserId, string RoleName) : IRequest;
