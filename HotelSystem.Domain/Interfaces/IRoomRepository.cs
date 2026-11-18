using HotelSystem.Domain.Entities;
namespace HotelSystem.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(Guid hotelId, string roomNumber,
        Guid? excludedRoomId = null, CancellationToken cancellationToken = default);
    void Add(Room room);
}
