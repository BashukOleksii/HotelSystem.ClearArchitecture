using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.ChangeMyPassword;

public sealed record ChangeMyPasswordCommand(Guid ActorId, string CurrentPassword, string NewPassword) : IRequest;
