using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewForModerationById;

public sealed class GetReviewForModerationByIdQueryHandler(IUserRepository users, IReviewReadService reads) : IRequestHandler<GetReviewForModerationByIdQuery, ReviewModerationDto>
{
    public async Task<ReviewModerationDto> Handle(GetReviewForModerationByIdQuery request, CancellationToken cancellationToken)
    {
        await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        return await reads.GetForModerationByIdAsync(request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Відгук не знайдено.");
    }
}
