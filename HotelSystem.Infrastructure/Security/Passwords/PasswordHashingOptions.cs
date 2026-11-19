namespace HotelSystem.Infrastructure.Security.Passwords;

public sealed class PasswordHashingOptions
{
    public const string SectionName = "PasswordHashing";
    public const int MinimumWorkFactor = 10;
    public const int MaximumWorkFactor = 16;

    public int WorkFactor { get; set; } = 12;
}
