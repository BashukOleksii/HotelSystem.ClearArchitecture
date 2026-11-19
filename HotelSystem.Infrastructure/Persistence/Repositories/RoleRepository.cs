using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository(HotelDbContext db) : IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var key = IdentityNormalization.Key(name);
        return await db.Roles.SingleOrDefaultAsync(x => EF.Property<string>(x, "NormalizedName") == key, cancellationToken);
    }
    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); return await db.Roles.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }
}
