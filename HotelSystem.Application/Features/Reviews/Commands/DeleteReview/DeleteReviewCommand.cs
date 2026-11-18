using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReview;

public sealed record DeleteReviewCommand(Guid ActorId, Guid ReviewId) : IRequest;
