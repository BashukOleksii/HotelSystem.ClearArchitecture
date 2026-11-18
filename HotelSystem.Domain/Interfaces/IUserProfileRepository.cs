using HotelSystem.Domain.Entities;

namespace HotelSystem.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<OwnerProfile?> GetOwnerAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<AdminProfile?> GetAdminAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    void Add(OwnerProfile profile);

    void Add(AdminProfile profile);
}