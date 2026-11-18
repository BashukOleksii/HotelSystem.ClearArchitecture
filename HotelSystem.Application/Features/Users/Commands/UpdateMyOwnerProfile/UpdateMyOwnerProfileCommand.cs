using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyOwnerProfile;

public sealed record UpdateMyOwnerProfileCommand(Guid ActorId, string? CompanyName = null, string? BusinessPhone = null) : IRequest;
