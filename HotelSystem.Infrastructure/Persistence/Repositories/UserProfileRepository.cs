using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class UserProfileRepository(HotelDbContext db) : IUserProfileRepository
{
    public async Task<OwnerProfile?> GetOwnerAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); return await db.OwnerProfiles.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
    public async Task<AdminProfile?> GetAdminAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); return await db.AdminProfiles.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
    public void Add(OwnerProfile profile) { db.EnsureUsable(); db.OwnerProfiles.Add(profile); }
    public void Add(AdminProfile profile) { db.EnsureUsable(); db.AdminProfiles.Add(profile); }
}
