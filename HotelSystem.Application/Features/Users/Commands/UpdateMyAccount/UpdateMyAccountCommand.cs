using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAccount;

public sealed record UpdateMyAccountCommand(Guid ActorId, UserDetailsInput Details, string CurrentPassword) : IRequest;
