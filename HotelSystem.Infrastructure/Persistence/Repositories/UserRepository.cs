using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(HotelDbContext db) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); return await db.Users.Include(x => x.Roles).AsSingleQuery().SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var key = IdentityNormalization.Key(email);
        return await db.Users.Include(x => x.Roles).AsSingleQuery().SingleOrDefaultAsync(x => EF.Property<string>(x, "NormalizedEmail") == key, cancellationToken);
    }
    public async Task<bool> ExistsByUsernameAsync(string username, Guid? excludedUserId = null, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var key = IdentityNormalization.Key(username);
        return await db.Users.AnyAsync(x => x.Id != excludedUserId && EF.Property<string>(x, "NormalizedUsername") == key, cancellationToken);
    }
    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludedUserId = null, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var key = IdentityNormalization.Key(email);
        return await db.Users.AnyAsync(x => x.Id != excludedUserId && EF.Property<string>(x, "NormalizedEmail") == key, cancellationToken);
    }
    public async Task<int> CountActiveAdminsAsync(CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Users.CountAsync(x => !x.IsBlocked && x.Roles.Any(r => r.Name == HotelSystem.Domain.Constants.RoleNames.Admin), cancellationToken);
    }
    public void Add(User entity) { db.EnsureUsable(); db.Users.Add(entity); }
}
