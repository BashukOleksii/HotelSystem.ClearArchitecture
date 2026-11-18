using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviews;

public sealed record GetMyReviewsQuery(Guid ActorId, ReviewFilter? Filter = null) : IRequest<PagedResult<ReviewDto>>;
