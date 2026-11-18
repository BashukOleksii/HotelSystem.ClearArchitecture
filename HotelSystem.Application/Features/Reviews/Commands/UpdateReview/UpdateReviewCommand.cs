using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.UpdateReview;

public sealed record UpdateReviewCommand(Guid ActorId, Guid ReviewId, int Rating, string? Comment = null) : IRequest;
