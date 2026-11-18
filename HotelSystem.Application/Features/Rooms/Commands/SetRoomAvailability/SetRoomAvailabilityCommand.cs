using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.SetRoomAvailability;
public sealed record SetRoomAvailabilityCommand(Guid ActorId, Guid RoomId, bool IsAvailable) : IRequest;
