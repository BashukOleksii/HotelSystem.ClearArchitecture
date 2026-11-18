using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomPrice;
public sealed class ChangeRoomPriceCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork) : IRequestHandler<ChangeRoomPriceCommand>
{
    public async Task Handle(ChangeRoomPriceCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);
        room.ChangePrice(request.CostPerNight);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
