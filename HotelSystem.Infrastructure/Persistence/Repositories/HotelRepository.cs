using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class HotelRepository(HotelDbContext db) : IHotelRepository
{
    public async Task<Hotel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Hotels.Include(x => x.Photos).AsSingleQuery().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public void Add(Hotel entity) { db.EnsureUsable(); db.Hotels.Add(entity); }
}
