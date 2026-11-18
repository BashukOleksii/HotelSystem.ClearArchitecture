using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.RemoveRoomPhoto;
public sealed class RemoveRoomPhotoCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork, IPhotoCleanupQueue cleanup) 
    : IRequestHandler<RemoveRoomPhotoCommand>
{
    public async Task Handle(RemoveRoomPhotoCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);

        if (!room.Photos.Any(x => x.Id == request.PhotoId)) throw new NotFoundException("Фотографію не знайдено.");

        var photo = room.RemovePhoto(request.PhotoId);

        cleanup.Enqueue(photo.StorageKey);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
