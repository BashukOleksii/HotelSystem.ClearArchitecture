namespace HotelSystem.Infrastructure.Persistence;

internal static class IdentityNormalization
{
    public const string OrdinalCollation = "Latin1_General_100_BIN2";
    public const string RoomNumberCollation = "Latin1_General_100_CI_AS";
    public static string Key(string value) => value.Trim().ToUpperInvariant();
}
