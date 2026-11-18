using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.UpdateBookingDates;

public sealed record UpdateBookingDatesCommand(Guid ActorId, Guid BookingId, DateOnly? CheckIn = null, DateOnly? CheckOut = null) : IRequest;
