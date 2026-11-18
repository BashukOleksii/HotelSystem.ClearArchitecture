using HotelSystem.Domain.Enums;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.ModerateHotel;

public sealed record ModerateHotelCommand(
    Guid ActorId,
    Guid HotelId,
    HotelModerationStatus Status) : IRequest;