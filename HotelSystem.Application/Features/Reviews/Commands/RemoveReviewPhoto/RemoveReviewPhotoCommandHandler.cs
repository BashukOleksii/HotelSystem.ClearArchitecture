using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.RemoveReviewPhoto;

public sealed class RemoveReviewPhotoCommandHandler(IUserRepository users, IReviewRepository reviews, IUnitOfWork unitOfWork, IPhotoCleanupQueue cleanup)
    : IRequestHandler<RemoveReviewPhotoCommand>
{
    public async Task Handle(RemoveReviewPhotoCommand request, CancellationToken cancellationToken)
    {
        var review = await ReviewAccess.OwnedAsync(request.ActorId, request.ReviewId, users, reviews, ct: cancellationToken);
        if (!review.Photos.Any(x => x.Id == request.PhotoId)) throw new NotFoundException("Фотографію не знайдено.");
        var photo = review.RemovePhoto(request.PhotoId);
        cleanup.Enqueue(photo.StorageKey);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
