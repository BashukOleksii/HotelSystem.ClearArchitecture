using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.Common;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.Rules;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.AddHotelPhoto;

public sealed class AddHotelPhotoCommandHandler(IUserRepository users, IHotelRepository hotels,
    IUnitOfWork unitOfWork, IPhotoStorage storage) : IRequestHandler<AddHotelPhotoCommand, PhotoDto>
{
    public async Task<PhotoDto> Handle(AddHotelPhotoCommand request, CancellationToken cancellationToken)
    {
        var hotel = await HotelAccess.OwnedAsync(request.ActorId, request.HotelId, users, hotels, cancellationToken);

        if (hotel.Photos.Count >= HotelRules.MaximumPhotoCount)
            throw new ConflictException($"Готель може мати максимум {HotelRules.MaximumPhotoCount} фотографій.");

        var file = await storage.UploadAsync(request.Upload, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        var photo = hotel.AddPhoto(file.StorageKey);

        // One atomic persistence operation for the parent version, Pending status and photo metadata.
        // An unreferenced upload after failure is handled by storage reconciliation, never eager deletion.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PhotoDto(photo.Id, file.Url);
    }
}
