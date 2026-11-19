using HotelSystem.Application.Common.Interfaces;

namespace HotelSystem.Infrastructure.Persistence.Outbox;

public sealed class EfPhotoCleanupQueue(HotelDbContext db, TimeProvider clock) : IPhotoCleanupQueue
{
    public void Enqueue(string storageKey)
    {
        db.EnsureUsable();
        db.PhotoCleanupJobs.Add(PhotoCleanupJob.Create(storageKey, clock.GetUtcNow()));
    }
}
