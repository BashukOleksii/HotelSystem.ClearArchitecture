using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.CreateBooking;

public sealed record CreateBookingCommand(Guid ActorId, Guid RoomId, DateOnly CheckIn, DateOnly CheckOut) : IRequest<Guid>;
