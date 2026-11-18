using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewsForModeration;

public sealed class GetReviewsForModerationQueryHandler(IUserRepository users, IReviewReadService reads) : IRequestHandler<GetReviewsForModerationQuery, PagedResult<ReviewModerationDto>>
{
    public async Task<PagedResult<ReviewModerationDto>> Handle(GetReviewsForModerationQuery request, CancellationToken cancellationToken)
    {
        await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        return await reads.GetModerationListAsync((request.Filter ?? new ReviewFilter()).Normalize(),
            request.UserId, request.IncludeDeleted, cancellationToken);
    }
}
