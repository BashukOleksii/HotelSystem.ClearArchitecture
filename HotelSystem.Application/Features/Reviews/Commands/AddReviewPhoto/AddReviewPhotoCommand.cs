using HotelSystem.Application.Common.Models;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Commands.AddReviewPhoto;

public sealed record AddReviewPhotoCommand(Guid ActorId, Guid ReviewId, PhotoUpload Upload) : IRequest<PhotoDto>;
