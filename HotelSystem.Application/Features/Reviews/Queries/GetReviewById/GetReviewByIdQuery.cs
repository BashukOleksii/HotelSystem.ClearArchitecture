using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewById;

public sealed record GetReviewByIdQuery(Guid ReviewId) : IRequest<ReviewDto>;
