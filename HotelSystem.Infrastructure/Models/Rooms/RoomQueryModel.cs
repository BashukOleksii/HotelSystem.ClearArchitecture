using HotelSystem.Domain.Entities;

namespace HotelSystem.Infrastructure.Models.Rooms;

internal sealed class RoomQueryModel
{
    public Room Room { get; init; } = null!;
    public Hotel Hotel { get; init; } = null!;
}
