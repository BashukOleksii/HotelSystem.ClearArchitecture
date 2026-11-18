using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed record CreateHotelCommand(
    Guid ActorId,
    string Name,
    string City,
    string Street,
    string Number,
    string? Description = null) : IRequest<Guid>;