using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetMyBookings;

public sealed record GetMyBookingsQuery(Guid ActorId, BookingFilter? Filter = null) : IRequest<PagedResult<BookingListItemDto>>;
