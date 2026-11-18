using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviews;

public sealed record GetHotelReviewsQuery(Guid HotelId, ReviewFilter? Filter = null) : IRequest<PagedResult<ReviewDto>>;
