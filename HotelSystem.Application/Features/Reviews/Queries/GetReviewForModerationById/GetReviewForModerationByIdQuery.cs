using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewForModerationById;

public sealed record GetReviewForModerationByIdQuery(Guid ActorId, Guid ReviewId) : IRequest<ReviewModerationDto>;
