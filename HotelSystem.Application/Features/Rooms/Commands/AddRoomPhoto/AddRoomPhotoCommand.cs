using HotelSystem.Application.Common.Models;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.AddRoomPhoto;
public sealed record AddRoomPhotoCommand(Guid ActorId, Guid RoomId, PhotoUpload Upload) : IRequest<PhotoDto>;
