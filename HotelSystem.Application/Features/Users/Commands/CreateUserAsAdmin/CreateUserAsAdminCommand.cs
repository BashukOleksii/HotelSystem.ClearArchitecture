using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.CreateUserAsAdmin;

public sealed record CreateUserAsAdminCommand(Guid ActorId, UserDetailsInput Details, string Password) : IRequest<Guid>;
