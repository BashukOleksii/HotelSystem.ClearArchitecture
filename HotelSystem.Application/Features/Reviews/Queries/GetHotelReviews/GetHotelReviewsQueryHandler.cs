using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviews;

public sealed class GetHotelReviewsQueryHandler(IHotelRepository hotels, IReviewReadService reads) : IRequestHandler<GetHotelReviewsQuery, PagedResult<ReviewDto>>
{
    public async Task<PagedResult<ReviewDto>> Handle(GetHotelReviewsQuery request, CancellationToken cancellationToken)
    {
        await ReviewAccess.PublicHotelAsync(request.HotelId, hotels, cancellationToken);
        return await reads.GetPublicHotelListAsync(request.HotelId, (request.Filter ?? new ReviewFilter()).Normalize(), cancellationToken);
    }
}
