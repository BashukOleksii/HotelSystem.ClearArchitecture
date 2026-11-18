using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewsForModeration;

public sealed record GetReviewsForModerationQuery(Guid ActorId, ReviewFilter? Filter = null, Guid? UserId = null, bool IncludeDeleted = false) : IRequest<PagedResult<ReviewModerationDto>>;
