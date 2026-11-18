using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.Rules;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.AddRoomPhoto;
public sealed class AddRoomPhotoCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork, IPhotoStorage storage) : IRequestHandler<AddRoomPhotoCommand, PhotoDto>
{
    public async Task<PhotoDto> Handle(AddRoomPhotoCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);
        if (room.Photos.Count >= RoomRules.MaximumPhotoCount)
            throw new ConflictException("Номер може мати максимум 10 фотографій.");
        var file = await storage.UploadAsync(request.Upload, cancellationToken);
        var photo = room.AddPhoto(file.StorageKey);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new PhotoDto(photo.Id, file.Url);
    }
}
