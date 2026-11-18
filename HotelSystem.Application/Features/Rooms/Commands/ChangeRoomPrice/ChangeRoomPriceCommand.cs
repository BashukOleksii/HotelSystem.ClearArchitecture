using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomPrice;
public sealed record ChangeRoomPriceCommand(Guid ActorId, Guid RoomId, decimal CostPerNight) : IRequest;
