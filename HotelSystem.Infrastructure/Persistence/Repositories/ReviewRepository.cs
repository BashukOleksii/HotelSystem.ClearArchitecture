using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository(HotelDbContext db) : IReviewRepository
{
    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Reviews.Include(x => x.Photos).AsSingleQuery().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public void Add(Review entity) { db.EnsureUsable(); db.Reviews.Add(entity); }
    public async Task<bool> ExistsForUserAsync(Guid hotelId, Guid userId, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Reviews.AnyAsync(x => x.HotelId == hotelId && x.UserId == userId && !x.IsDeleted, cancellationToken);
    }
}
