using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.DeleteRoom;
public sealed class DeleteRoomCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IBookingRepository bookings, IUnitOfWork unitOfWork, BookingCalendar calendar) : IRequestHandler<DeleteRoomCommand>
{
    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, ct);
            if (await bookings.HasActiveForRoomAsync(room.Id, calendar.Today, ct))
                throw new ConflictException("Номер має поточні або майбутні підтверджені бронювання.");
            room.Delete();
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
