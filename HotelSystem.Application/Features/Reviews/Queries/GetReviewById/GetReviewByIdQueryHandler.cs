using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewById;

public sealed class GetReviewByIdQueryHandler(IReviewReadService reads) : IRequestHandler<GetReviewByIdQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return await reads.GetPublicByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Відгук не знайдено.");
    }
}
