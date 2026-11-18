using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateUserAsAdmin;

public sealed record UpdateUserAsAdminCommand(Guid ActorId, Guid UserId, UserDetailsInput Details) : IRequest;
