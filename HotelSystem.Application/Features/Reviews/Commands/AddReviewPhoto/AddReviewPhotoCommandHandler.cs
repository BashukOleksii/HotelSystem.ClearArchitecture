using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.Rules;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.AddReviewPhoto;

public sealed class AddReviewPhotoCommandHandler(IUserRepository users, IReviewRepository reviews, IUnitOfWork unitOfWork, IPhotoStorage storage)
    : IRequestHandler<AddReviewPhotoCommand, PhotoDto>
{
    public async Task<PhotoDto> Handle(AddReviewPhotoCommand request, CancellationToken cancellationToken)
    {
        var review = await ReviewAccess.OwnedAsync(request.ActorId, request.ReviewId, users, reviews, ct: cancellationToken);
        if (review.Photos.Count >= ReviewRules.MaximumPhotoCount)
            throw new ConflictException("Відгук може мати максимум 5 фотографій.");
        var file = await storage.UploadAsync(request.Upload, cancellationToken);
        var photo = review.AddPhoto(file.StorageKey);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new PhotoDto(photo.Id, file.Url);
    }
}
