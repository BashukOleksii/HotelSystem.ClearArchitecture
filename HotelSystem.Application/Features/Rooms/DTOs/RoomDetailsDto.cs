using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Rooms.DTOs;
public sealed record RoomDetailsDto(Guid Id, Guid HotelId, string HotelName,
    string City, string Street, string Number, string RoomNumber, RoomType Type,
    decimal CostPerNight, int Capacity, bool IsAvailable,
    bool? IsAvailableForDates, IReadOnlyList<PhotoDto> Photos);
