using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Rooms.DTOs;
public sealed record RoomManagementDto(Guid Id, Guid HotelId, Guid OwnerId, string HotelName,
    HotelModerationStatus HotelModerationStatus, string RoomNumber, RoomType Type,
    decimal CostPerNight, int Capacity, bool IsAvailable, DateTimeOffset CreatedAt,
    Guid Version, IReadOnlyList<PhotoDto> Photos);
