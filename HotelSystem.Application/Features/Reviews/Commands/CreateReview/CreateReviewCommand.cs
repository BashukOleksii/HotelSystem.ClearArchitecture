using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(Guid ActorId, Guid HotelId, int Rating, string? Comment = null) : IRequest<Guid>;
