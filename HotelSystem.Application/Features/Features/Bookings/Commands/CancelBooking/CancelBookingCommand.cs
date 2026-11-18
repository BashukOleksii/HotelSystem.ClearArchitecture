using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.CancelBooking;

public sealed record CancelBookingCommand(Guid ActorId, Guid BookingId) : IRequest;
