using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandHandler(IUserRepository users, IReviewRepository reviews, IUnitOfWork unitOfWork, TimeProvider clock)
    : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await ReviewAccess.OwnedAsync(request.ActorId, request.ReviewId, users, reviews,
            includeDeleted: true, ct: cancellationToken);
        if (review.IsDeleted) return;
        review.Delete(request.ActorId, clock.GetUtcNow());
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
