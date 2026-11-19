using HotelSystem.Domain.Enums;
using HotelSystem.Infrastructure.DTOs.Photos;

namespace HotelSystem.Infrastructure.DTOs.Bookings;

internal sealed record BookingReadData(Guid Id, Guid RoomId, Guid HotelId, Guid OwnerId,
    Guid UserId, string Username, string Email, string HotelName, string RoomNumber,
    string City, string Street, string Number, DateOnly CheckIn, DateOnly CheckOut,
    decimal PricePerNight, decimal TotalPrice, BookingStatus Status, DateTimeOffset CreatedAt,
    bool HotelIsDeleted, bool RoomIsDeleted, bool RoomIsAvailable,
    HotelModerationStatus HotelModerationStatus, List<PhotoReadData> RoomPhotos)
{
    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;
}
