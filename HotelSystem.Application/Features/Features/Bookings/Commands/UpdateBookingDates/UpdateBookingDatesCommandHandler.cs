using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.Rules;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.UpdateBookingDates;

public sealed class UpdateBookingDatesCommandHandler(IUserRepository users, IRoomRepository rooms, IHotelRepository hotels,
    IBookingRepository bookings, IUnitOfWork unitOfWork, BookingCalendar calendar)
    : IRequestHandler<UpdateBookingDatesCommand>
{
    public async Task Handle(UpdateBookingDatesCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var booking = await BookingAccess.OwnedBookingAsync(request.ActorId, request.BookingId, users, bookings, ct);
            if (booking.Status == BookingStatus.Cancelled)
                throw new ConflictException("Скасоване бронювання не можна редагувати.");
            var checkIn = request.CheckIn ?? booking.CheckIn;
            var checkOut = request.CheckOut ?? booking.CheckOut;
            if (checkIn == booking.CheckIn && checkOut == booking.CheckOut)
                return; 
            BookingRules.EnsureDates(checkIn, checkOut, calendar.Today);
            var room = await BookingAccess.BookableRoomAsync(booking.RoomId, rooms, hotels, ct);
            if (await bookings.HasOverlapAsync(room.Id, checkIn, checkOut, booking.Id, ct))
                throw new ConflictException("Номер уже заброньований на обрані дати.");
            booking.UpdateDates(room, checkIn, checkOut, calendar.Today);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
