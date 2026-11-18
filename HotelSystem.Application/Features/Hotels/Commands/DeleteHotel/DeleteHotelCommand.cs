using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed record DeleteHotelCommand(
    Guid ActorId,
    Guid HotelId) : IRequest;