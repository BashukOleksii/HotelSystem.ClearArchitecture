using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AccessTokenDto>;
