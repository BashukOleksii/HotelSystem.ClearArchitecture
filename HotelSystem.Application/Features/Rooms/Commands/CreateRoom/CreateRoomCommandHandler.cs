using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.CreateRoom;
public sealed class CreateRoomCommandHandler(IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, IUnitOfWork unitOfWork, TimeProvider clock) : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var hotel = await RoomAccess.OwnedHotelAsync(request.ActorId, request.HotelId, users, hotels, cancellationToken);
        var input = request.Details;
        if (await rooms.ExistsByNumberAsync(hotel.Id, input.RoomNumber.Trim(), cancellationToken: cancellationToken))
            throw new ConflictException("Такий номер кімнати вже використовується в цьому готелі.");
        var room = Room.Create(hotel.Id, input.RoomNumber, input.Type, input.CostPerNight,
            input.Capacity, clock.GetUtcNow(), input.IsAvailable);
        rooms.Add(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return room.Id;
    }
}
