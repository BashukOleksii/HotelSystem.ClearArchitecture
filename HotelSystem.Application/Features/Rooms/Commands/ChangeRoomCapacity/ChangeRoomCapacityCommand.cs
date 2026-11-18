using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomCapacity;
public sealed record ChangeRoomCapacityCommand(Guid ActorId, Guid RoomId, int Capacity) : IRequest;
