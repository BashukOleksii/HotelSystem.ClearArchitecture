using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.RemoveHotelPhoto;

public sealed record RemoveHotelPhotoCommand(Guid ActorId, Guid HotelId, Guid PhotoId) : IRequest;
