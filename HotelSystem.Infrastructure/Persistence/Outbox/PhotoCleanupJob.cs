namespace HotelSystem.Infrastructure.Persistence.Outbox;

public sealed class PhotoCleanupJob
{
    public Guid Id { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset NextAttemptAt { get; private set; }
    public int Attempts { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public DateTimeOffset? LockedUntil { get; private set; }
    public Guid? LeaseId { get; private set; }
    public string? LastError { get; private set; }
    public Guid Version { get; private set; }
    private PhotoCleanupJob() { }

    internal static PhotoCleanupJob Create(string key, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (key.Length > 512) throw new ArgumentOutOfRangeException(nameof(key));
        return new() { Id = Guid.NewGuid(), StorageKey = key, CreatedAt = now, NextAttemptAt = now, Version = Guid.NewGuid() };
    }
}
