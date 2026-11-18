using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Hotels.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.RemoveHotelPhoto;

public sealed class RemoveHotelPhotoCommandHandler(IUserRepository users, IHotelRepository hotels,
    IUnitOfWork unitOfWork, IPhotoCleanupQueue cleanup) : IRequestHandler<RemoveHotelPhotoCommand>
{
    public async Task Handle(RemoveHotelPhotoCommand request, CancellationToken cancellationToken)
    {
        var hotel = await HotelAccess.OwnedAsync(request.ActorId, request.HotelId, users, hotels, cancellationToken);

        if (!hotel.Photos.Any(x => x.Id == request.PhotoId))
            throw new NotFoundException("Фотографію не знайдено в галереї цього готелю.");

        var photo = hotel.RemovePhoto(request.PhotoId);
        cleanup.Enqueue(photo.StorageKey);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
