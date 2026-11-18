using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed record UpdateHotelCommand(
    Guid ActorId,
    Guid HotelId,
    string Name,
    string City,
    string Street,
    string Number,
    string? Description = null) : IRequest;