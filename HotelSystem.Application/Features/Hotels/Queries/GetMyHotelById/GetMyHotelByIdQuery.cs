using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotelById;

public sealed record GetMyHotelByIdQuery(
    Guid ActorId,
    Guid HotelId) : IRequest<HotelManagementDetailsDto>;