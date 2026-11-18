using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviews;

public sealed record GetOwnerReviewsQuery(Guid ActorId, ReviewFilter? Filter = null) : IRequest<PagedResult<ReviewDto>>;
