using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public sealed class GetHotelReviewSummaryQueryHandler(IHotelRepository hotels, IReviewReadService reads) : IRequestHandler<GetHotelReviewSummaryQuery, HotelReviewSummaryDto>
{
    public async Task<HotelReviewSummaryDto> Handle(GetHotelReviewSummaryQuery request, CancellationToken cancellationToken)
    {
        await ReviewAccess.PublicHotelAsync(request.HotelId, hotels, cancellationToken);
        return await reads.GetPublicHotelSummaryAsync(request.HotelId, cancellationToken);
    }
}
