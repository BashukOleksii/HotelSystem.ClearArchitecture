using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Reviews.Common;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviewById;

public sealed class GetMyReviewByIdQueryHandler(IUserRepository users, IReviewReadService reads) : IRequestHandler<GetMyReviewByIdQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetMyReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await ReviewAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, cancellationToken);
        return await reads.GetClientByIdAsync(actor.Id, request.ReviewId, cancellationToken)
            ?? throw new NotFoundException("Відгук не знайдено.");
    }
}
