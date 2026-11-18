using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviewById;

public sealed class GetOwnerReviewByIdQueryHandler(IUserRepository users, IReviewReadService reads) : IRequestHandler<GetOwnerReviewByIdQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetOwnerReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.Owner, users, cancellationToken);
        return await reads.GetOwnerByIdAsync(actor.Id, request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Відгук не знайдено.");
    }
}
