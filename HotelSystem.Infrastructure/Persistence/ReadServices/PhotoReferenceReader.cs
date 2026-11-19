using HotelSystem.Application.Features.Photos.Queries;
using Microsoft.EntityFrameworkCore;
namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class PhotoReferenceReader(HotelDbContext db) : IPhotoReferenceReader
{
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Hotels.AsNoTracking().AnyAsync(h => !h.IsDeleted && h.Photos.Any(p => p.StorageKey == key), cancellationToken)
            || await db.Rooms.AsNoTracking().AnyAsync(r => !r.IsDeleted && r.Photos.Any(p => p.StorageKey == key)
                && db.Hotels.Any(h => h.Id == r.HotelId && !h.IsDeleted), cancellationToken)
            || await db.Reviews.AsNoTracking().AnyAsync(r => !r.IsDeleted && r.Photos.Any(p => p.StorageKey == key)
                && db.Hotels.Any(h => h.Id == r.HotelId && !h.IsDeleted), cancellationToken);
    }
}
