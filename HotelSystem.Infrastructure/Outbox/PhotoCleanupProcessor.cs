using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Infrastructure.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Persistence.Outbox;

public sealed class PhotoCleanupProcessor(HotelDbContext db, IPhotoStorage storage, TimeProvider clock, IOptions<PhotoStorageOptions> options)
{
    public async Task<int> ProcessBatchAsync(CancellationToken ct = default)
    {
        db.EnsureUsable();
        var processed = 0;
        for (var i = 0; i < options.Value.BatchSize; i++)
        {
            ct.ThrowIfCancellationRequested();
            var now = clock.GetUtcNow();
            var due = db.PhotoCleanupJobs.AsNoTracking().Where(x => x.CompletedAt == null && x.NextAttemptAt <= now
                && (x.LockedUntil == null || x.LockedUntil <= now));
            var candidate = await due.OrderBy(x => x.NextAttemptAt).ThenBy(x => x.Id).FirstOrDefaultAsync(ct);
            if (candidate is null) break;

            var leaseId = Guid.NewGuid(); var version = Guid.NewGuid(); var until = now.AddSeconds(options.Value.LeaseSeconds);
            var claimed = await due.Where(x => x.Id == candidate.Id && x.Version == candidate.Version)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.LeaseId, leaseId)
                    .SetProperty(x => x.LockedUntil, until).SetProperty(x => x.Version, version)
                    .SetProperty(x => x.Attempts, x => x.Attempts < int.MaxValue ? x.Attempts + 1 : x.Attempts), ct);
            if (claimed == 0) continue;
            processed++;
            string? retryReason = null;
            try
            {
                if (await IsReferencedAsync(candidate.StorageKey, ct)) retryReason = "StillReferenced";
                else await storage.DeleteAsync(candidate.StorageKey, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
            catch (ArgumentException) { retryReason = "InvalidStorageKey"; }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException)
            { retryReason = error.GetType().Name; }

            var owned = db.PhotoCleanupJobs.Where(x => x.Id == candidate.Id && x.LeaseId == leaseId && x.Version == version);
            if (retryReason is null)
            {
                var completedAt = clock.GetUtcNow();
                await owned.ExecuteUpdateAsync(s => s.SetProperty(x => x.CompletedAt, completedAt)
                    .SetProperty(x => x.LockedUntil, (DateTimeOffset?)null).SetProperty(x => x.LeaseId, (Guid?)null)
                    .SetProperty(x => x.LastError, (string?)null).SetProperty(x => x.Version, Guid.NewGuid()), ct);
            }
            else
            {
                var next = clock.GetUtcNow().AddSeconds(Math.Min(3600, 5 * Math.Pow(2, Math.Min(candidate.Attempts + 1L, 10))));
                await owned.ExecuteUpdateAsync(s => s.SetProperty(x => x.NextAttemptAt, next)
                    .SetProperty(x => x.LockedUntil, (DateTimeOffset?)null).SetProperty(x => x.LeaseId, (Guid?)null)
                    .SetProperty(x => x.LastError, retryReason).SetProperty(x => x.Version, Guid.NewGuid()), ct);
            }
        }
        return processed;
    }

    private async Task<bool> IsReferencedAsync(string key, CancellationToken ct) =>
        await db.Hotels.AsNoTracking().AnyAsync(x => x.Photos.Any(p => p.StorageKey == key), ct)
        || await db.Rooms.AsNoTracking().AnyAsync(x => x.Photos.Any(p => p.StorageKey == key), ct)
        || await db.Reviews.AsNoTracking().AnyAsync(x => x.Photos.Any(p => p.StorageKey == key), ct);
}
