using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> HasOverlapAsync(
        Guid roomId,
        DateOnly checkIn,
        DateOnly checkOut,
        Guid? excludedBookingId = null,
        CancellationToken cancellationToken = default);

    void Add(Booking booking);
}