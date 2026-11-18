using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandHandler(IUserRepository users, IReviewRepository reviews, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateReviewCommand>
{
    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await ReviewAccess.OwnedAsync(request.ActorId, request.ReviewId, users, reviews, ct: cancellationToken);
        var version = review.Version;
        review.UpdateDetails(request.Rating, request.Comment);
        if (review.Version != version) await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
