using HotelSystem.Domain.Enums;
using HotelSystem.Infrastructure.DTOs.Photos;

namespace HotelSystem.Infrastructure.DTOs.Rooms;

internal sealed record RoomReadData(Guid Id, Guid HotelId, Guid OwnerId, string HotelName,
    HotelModerationStatus HotelModerationStatus, string City, string Street, string Number,
    string RoomNumber, RoomType Type, decimal CostPerNight, int Capacity, bool IsAvailable,
    DateTimeOffset CreatedAt, Guid Version, bool? IsAvailableForDates, List<PhotoReadData> Photos);
