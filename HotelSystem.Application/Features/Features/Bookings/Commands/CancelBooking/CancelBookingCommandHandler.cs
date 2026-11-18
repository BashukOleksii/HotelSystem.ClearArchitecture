using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Commands.CancelBooking;

public sealed class CancelBookingCommandHandler(IUserRepository users, IBookingRepository bookings, IUnitOfWork unitOfWork)
    : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var booking = await BookingAccess.OwnedBookingAsync(request.ActorId, request.BookingId, users, bookings, ct);
            if (booking.Status == BookingStatus.Cancelled)
                return;
            booking.Cancel();
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
