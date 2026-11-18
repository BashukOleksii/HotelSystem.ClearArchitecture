using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReviewAsAdmin;

public sealed class DeleteReviewAsAdminCommandHandler(IUserRepository users, IReviewRepository reviews, IUnitOfWork unitOfWork, TimeProvider clock)
    : IRequestHandler<DeleteReviewAsAdminCommand>
{
    public async Task Handle(DeleteReviewAsAdminCommand request, CancellationToken cancellationToken)
    {
        var actor = await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        var review = await reviews.GetByIdAsync(request.ReviewId, cancellationToken);
        if (review is null) throw new NotFoundException("Відгук не знайдено.");
        if (review.IsDeleted) return;
        review.Delete(actor.Id, clock.GetUtcNow());
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
