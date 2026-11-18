using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Role>> GetAllAsync(
        CancellationToken cancellationToken = default);
}