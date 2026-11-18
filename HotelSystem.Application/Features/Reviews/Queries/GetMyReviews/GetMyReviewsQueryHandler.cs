using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviews;

public sealed class GetMyReviewsQueryHandler(IUserRepository users, IReviewReadService reads) : IRequestHandler<GetMyReviewsQuery, PagedResult<ReviewDto>>
{
    public async Task<PagedResult<ReviewDto>> Handle(GetMyReviewsQuery request, CancellationToken cancellationToken)
    {
        var actor = await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, cancellationToken);
        return await reads.GetClientListAsync(actor.Id, (request.Filter ?? new ReviewFilter()).Normalize(), cancellationToken);
    }
}
