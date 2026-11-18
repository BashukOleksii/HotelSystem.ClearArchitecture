using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.RemoveRoomPhoto;
public sealed record RemoveRoomPhotoCommand(Guid ActorId, Guid RoomId, Guid PhotoId) : IRequest;
