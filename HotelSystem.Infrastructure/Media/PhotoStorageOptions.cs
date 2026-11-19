namespace HotelSystem.Infrastructure.Media;

public sealed class PhotoStorageOptions
{
    public const string SectionName = "PhotoStorage";
    public string RootDirectory { get; set; } = Path.Combine(AppContext.BaseDirectory, "App_Data", "photos");
    public bool EnableCleanupWorker { get; set; } = true;
    public int PollSeconds { get; set; } = 15;
    public int BatchSize { get; set; } = 20;
    public int LeaseSeconds { get; set; } = 120;

    internal bool IsValid() => !string.IsNullOrWhiteSpace(RootDirectory) && Path.IsPathFullyQualified(RootDirectory)
        && PollSeconds is >= 1 and <= 3600 && BatchSize is >= 1 and <= 100 && LeaseSeconds is >= 30 and <= 3600;
}
