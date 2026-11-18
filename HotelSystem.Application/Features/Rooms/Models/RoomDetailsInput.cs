using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Rooms.Models;
public sealed record RoomDetailsInput(string RoomNumber, RoomType Type,
    decimal CostPerNight, int Capacity, bool IsAvailable = true);
