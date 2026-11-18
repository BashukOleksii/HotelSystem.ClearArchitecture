using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviewById;

public sealed record GetMyReviewByIdQuery(Guid ActorId, Guid ReviewId) : IRequest<ReviewDto>;
