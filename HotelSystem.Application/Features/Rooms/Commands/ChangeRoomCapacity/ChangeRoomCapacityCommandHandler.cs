using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomCapacity;
public sealed class ChangeRoomCapacityCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork) : IRequestHandler<ChangeRoomCapacityCommand>
{
    public async Task Handle(ChangeRoomCapacityCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);
        room.ChangeCapacity(request.Capacity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
