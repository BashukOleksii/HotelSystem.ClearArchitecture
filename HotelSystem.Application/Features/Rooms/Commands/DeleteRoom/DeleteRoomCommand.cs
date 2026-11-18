using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.DeleteRoom;
public sealed record DeleteRoomCommand(Guid ActorId, Guid RoomId) : IRequest;
