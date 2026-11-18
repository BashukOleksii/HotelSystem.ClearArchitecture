using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.SetRoomAvailability;
public sealed class SetRoomAvailabilityCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork) : IRequestHandler<SetRoomAvailabilityCommand>
{
    public async Task Handle(SetRoomAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);
        if (request.IsAvailable) room.OpenForBooking(); else room.CloseForBooking();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
