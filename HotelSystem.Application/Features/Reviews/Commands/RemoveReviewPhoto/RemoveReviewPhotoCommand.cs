using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.RemoveReviewPhoto;

public sealed record RemoveReviewPhotoCommand(Guid ActorId, Guid ReviewId, Guid PhotoId) : IRequest;
