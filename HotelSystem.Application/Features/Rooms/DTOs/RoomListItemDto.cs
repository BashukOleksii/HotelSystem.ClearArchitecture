using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Rooms.DTOs;
public sealed record RoomListItemDto(Guid Id, Guid HotelId, string HotelName, string City,
    string RoomNumber, RoomType Type, decimal CostPerNight, int Capacity,
    bool IsAvailable, IReadOnlyList<PhotoDto> Photos);
