using HotelSystem.Application.Common.Models;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.AddHotelPhoto;

public sealed record AddHotelPhotoCommand(Guid ActorId, Guid HotelId, PhotoUpload Upload) : IRequest<PhotoDto>;
