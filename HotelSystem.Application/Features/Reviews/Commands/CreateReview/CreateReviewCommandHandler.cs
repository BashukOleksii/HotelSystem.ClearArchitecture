using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler(IUserRepository users, IHotelRepository hotels, IReviewRepository reviews, IUnitOfWork unitOfWork, TimeProvider clock)
    : IRequestHandler<CreateReviewCommand, Guid>
{
    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        Guid id = Guid.Empty;
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var actor = await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, ct);
            await ReviewAccess.PublicHotelAsync(request.HotelId, hotels, ct);
            if (await reviews.ExistsForUserAsync(request.HotelId, actor.Id, ct))
                throw new ConflictException("Ви вже залишили активний відгук для цього готелю.");
            var review = Review.Create(request.HotelId, actor.Id, request.Rating, request.Comment, clock.GetUtcNow());
            reviews.Add(review);
            await unitOfWork.SaveChangesAsync(ct);
            id = review.Id;
        }, cancellationToken);
        return id;
    }
}
