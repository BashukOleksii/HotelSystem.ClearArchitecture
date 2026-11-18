using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.SetUserBlocked;

public sealed record SetUserBlockedCommand(Guid ActorId, Guid UserId, bool IsBlocked) : IRequest;
