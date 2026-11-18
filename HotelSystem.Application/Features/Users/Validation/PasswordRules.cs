using System.Text;

namespace HotelSystem.Application.Features.Users.Validation;

public static class PasswordRules
{
    public const int MinimumNewPasswordCharacters = 15;
    public const int MaximumUtf8Bytes = 72;

    public static bool CanVerify(string? password)
    {
        if (string.IsNullOrEmpty(password) || password.Length > MaximumUtf8Bytes || password.Contains('\0')) return false;
        try { return new UTF8Encoding(false, true).GetByteCount(password) <= MaximumUtf8Bytes; }
        catch (EncoderFallbackException) { return false; }
    }

    public static bool IsValidNew(string? password) => CanVerify(password)
        && password!.EnumerateRunes().Count() >= MinimumNewPasswordCharacters;
}
