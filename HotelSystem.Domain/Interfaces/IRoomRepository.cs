using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    void Add(Room room);

    void Remove(Room room);
}