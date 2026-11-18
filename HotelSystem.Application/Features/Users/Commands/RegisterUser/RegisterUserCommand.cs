using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(UserDetailsInput Details, string Password) : IRequest<Guid>;
