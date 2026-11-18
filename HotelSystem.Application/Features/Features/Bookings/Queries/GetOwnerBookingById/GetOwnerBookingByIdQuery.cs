using HotelSystem.Application.Features.Bookings.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookingById;

public sealed record GetOwnerBookingByIdQuery(Guid ActorId, Guid BookingId) : IRequest<BookingManagementDto>;
