using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;

namespace HotelSystem.Application.Features.Rooms.Common;
internal static class RoomAccess
{
    public static async Task<User> RequireRoleAsync(Guid actorId, string role,
        IUserRepository users, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(actorId, ct);
        if (user is null || user.IsBlocked || !user.HasRole(role))
            throw new ForbiddenOperationException($"Операція доступна активному користувачу з роллю {role}.");
        return user;
    }
    public static async Task<Hotel> OwnedHotelAsync(Guid actorId, Guid hotelId,
        IUserRepository users, IHotelRepository hotels, CancellationToken ct)
    {
        var user = await RequireRoleAsync(actorId, RoleNames.Owner, users, ct);
        var hotel = await hotels.GetByIdAsync(hotelId, ct);
        if (hotel is null || hotel.IsDeleted) throw new NotFoundException("Готель не знайдено.");
        if (hotel.OwnerId != user.Id) throw new ForbiddenOperationException("Доступні лише власні готелі.");
        return hotel;
    }
    public static async Task<Room> OwnedRoomAsync(Guid actorId, Guid roomId,
        IUserRepository users, IHotelRepository hotels, IRoomRepository rooms, CancellationToken ct)
    {
        var user = await RequireRoleAsync(actorId, RoleNames.Owner, users, ct);
        var room = await rooms.GetByIdAsync(roomId, ct);
        if (room is null || room.IsDeleted) throw new NotFoundException("Номер не знайдено.");
        var hotel = await hotels.GetByIdAsync(room.HotelId, ct);
        if (hotel is null || hotel.IsDeleted) throw new NotFoundException("Готель не знайдено.");
        if (hotel.OwnerId != user.Id) throw new ForbiddenOperationException("Доступні лише номери власних готелів.");
        return room;
    }
}
