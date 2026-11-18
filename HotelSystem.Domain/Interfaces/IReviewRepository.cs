using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsForUserAsync(
        Guid hotelId,
        Guid userId,
        CancellationToken cancellationToken = default);

    void Add(Review review);

    void Remove(Review review);
}