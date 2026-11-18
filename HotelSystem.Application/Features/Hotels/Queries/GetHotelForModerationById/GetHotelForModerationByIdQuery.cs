using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelForModerationById;

public sealed record GetHotelForModerationByIdQuery(
    Guid ActorId,
    Guid HotelId) : IRequest<HotelManagementDetailsDto>;