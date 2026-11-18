using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReviewAsAdmin;

public sealed record DeleteReviewAsAdminCommand(Guid ActorId, Guid ReviewId) : IRequest;
