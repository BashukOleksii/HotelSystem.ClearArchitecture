using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Interfaces;

namespace HotelSystem.Application.Features.Bookings.Common;

internal static class BookingAccess
{
    public static async Task<User> RequireRoleAsync(Guid actorId, string role,
        IUserRepository users, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(actorId, ct);
        if (user is null || user.IsBlocked || !user.HasRole(role))
            throw new ForbiddenOperationException($"Операція доступна активному користувачу з роллю {role}.");
        return user;
    }

    public static async Task<Booking> OwnedBookingAsync(Guid actorId, Guid bookingId,
        IUserRepository users, IBookingRepository bookings, CancellationToken ct)
    {
        var user = await RequireRoleAsync(actorId, RoleNames.User, users, ct);
        var booking = await bookings.GetByIdAsync(bookingId, ct);
        if (booking is null)
            throw new NotFoundException("Бронювання не знайдено.");
        if (booking.UserId != user.Id)
            throw new ForbiddenOperationException("Ви можете змінювати лише власні бронювання.");
        return booking;
    }

    public static async Task<Room> BookableRoomAsync(Guid roomId, IRoomRepository rooms,
        IHotelRepository hotels, CancellationToken ct)
    {
        var room = await rooms.GetByIdAsync(roomId, ct);
        if (room is null || room.IsDeleted)
            throw new NotFoundException("Номер не знайдено.");
        var hotel = await hotels.GetByIdAsync(room.HotelId, ct);
        if (hotel is null || hotel.IsDeleted)
            throw new NotFoundException("Готель не знайдено.");
        if (hotel.ModerationStatus != HotelModerationStatus.Approved)
            throw new ConflictException("Готель недоступний для бронювання.");
        if (!room.IsAvailable)
            throw new ConflictException("Номер тимчасово закритий для бронювання.");
        return room;
    }
}
