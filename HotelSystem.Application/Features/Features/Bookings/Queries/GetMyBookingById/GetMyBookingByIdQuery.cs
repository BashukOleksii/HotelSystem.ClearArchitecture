using HotelSystem.Application.Features.Bookings.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetMyBookingById;

public sealed record GetMyBookingByIdQuery(Guid ActorId, Guid BookingId) : IRequest<BookingDetailsDto>;
