using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviewById;

public sealed record GetOwnerReviewByIdQuery(Guid ActorId, Guid ReviewId) : IRequest<ReviewDto>;
