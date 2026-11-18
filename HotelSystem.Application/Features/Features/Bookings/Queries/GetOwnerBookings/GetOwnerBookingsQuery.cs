using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookings;

public sealed record GetOwnerBookingsQuery(Guid ActorId, BookingFilter? Filter = null) : IRequest<PagedResult<BookingManagementDto>>;
