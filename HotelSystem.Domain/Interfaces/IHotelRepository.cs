using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    void Add(Hotel hotel);
}
