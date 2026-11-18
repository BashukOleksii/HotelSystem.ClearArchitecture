using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    void Add(User user);
}