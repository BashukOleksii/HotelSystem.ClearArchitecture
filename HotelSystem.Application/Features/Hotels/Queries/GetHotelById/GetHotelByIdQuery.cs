using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed record GetHotelByIdQuery(Guid HotelId)
    : IRequest<HotelDetailsDto>;