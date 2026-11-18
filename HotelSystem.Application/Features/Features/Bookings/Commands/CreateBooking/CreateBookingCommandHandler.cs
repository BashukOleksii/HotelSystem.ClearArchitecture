using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandHandler(IUserRepository users, IRoomRepository rooms, IHotelRepository hotels,
    IBookingRepository bookings, IUnitOfWork unitOfWork, BookingCalendar calendar, TimeProvider clock)
    : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        Guid bookingId = Guid.Empty;
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var actor = await BookingAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, ct);
            var room = await BookingAccess.BookableRoomAsync(request.RoomId, rooms, hotels, ct);
            var booking = Booking.Create(actor.Id, room, request.CheckIn, request.CheckOut,
                calendar.Today, clock.GetUtcNow());
            if (await bookings.HasOverlapAsync(room.Id, request.CheckIn, request.CheckOut, cancellationToken: ct))
                throw new ConflictException("Номер уже заброньований на обрані дати.");
            bookings.Add(booking);
            await unitOfWork.SaveChangesAsync(ct);
            bookingId = booking.Id;
        }, cancellationToken);
        return bookingId;
    }
}
