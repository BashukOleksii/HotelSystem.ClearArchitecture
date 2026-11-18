using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.UpdateRoom;
public sealed class UpdateRoomCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateRoomCommand>
{
    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await RoomAccess.OwnedRoomAsync(request.ActorId, request.RoomId, users, hotels, rooms, cancellationToken);

        var input = request.Details;

        if (await rooms.ExistsByNumberAsync(room.HotelId, input.RoomNumber.Trim(), room.Id, cancellationToken))
            throw new ConflictException("Такий номер кімнати вже використовується в цьому готелі.");

        room.UpdateDetails(input.RoomNumber, input.Type, input.CostPerNight, input.Capacity, input.IsAvailable);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
