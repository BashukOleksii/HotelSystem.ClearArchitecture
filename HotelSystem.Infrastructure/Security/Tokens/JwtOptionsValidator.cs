using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Security.Tokens;

public sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        var failures = new List<string>();
        if (!ValidIdentifier(options.Issuer)) failures.Add("Jwt:Issuer має містити від 1 до 256 символів без крайніх пробілів.");
        if (!ValidIdentifier(options.Audience)) failures.Add("Jwt:Audience має містити від 1 до 256 символів без крайніх пробілів.");
        if (options.AccessTokenMinutes is < 1 or > 60) failures.Add("Jwt:AccessTokenMinutes має бути від 1 до 60.");

        if (string.IsNullOrWhiteSpace(options.SigningKeyBase64) || options.SigningKeyBase64.Length > 172)
            failures.Add("Jwt:SigningKeyBase64 має містити Base64 ключа з 32–128 випадкових байтів.");
        else
        {
            Span<byte> buffer = stackalloc byte[128];
            if (!Convert.TryFromBase64String(options.SigningKeyBase64, buffer, out var written) || written is < 32 or > 128)
                failures.Add("Jwt:SigningKeyBase64 має містити Base64 ключа з 32–128 випадкових байтів.");
        }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }

    private static bool ValidIdentifier(string? value) => !string.IsNullOrWhiteSpace(value)
        && value.Length <= 256 && value == value.Trim() && !value.Any(char.IsControl);
}
