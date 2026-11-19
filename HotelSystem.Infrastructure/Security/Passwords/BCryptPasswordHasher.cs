using System.Security.Cryptography;
using System.Text.RegularExpressions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Validation;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Security.Passwords;

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    private static readonly Regex HashFormat = new(
        @"\A\$2[aby]\$(1[0-6])\$[./A-Za-z0-9]{53}\z", RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    private readonly int _workFactor;
    private readonly string _dummyHash;

    public BCryptPasswordHasher(IOptions<PasswordHashingOptions> options)
    {
        _workFactor = options.Value.WorkFactor;
        if (_workFactor is < PasswordHashingOptions.MinimumWorkFactor or > PasswordHashingOptions.MaximumWorkFactor)
            throw new OptionsValidationException(Options.DefaultName, typeof(PasswordHashingOptions),
                ["WorkFactor має бути від 10 до 16."]);

        _dummyHash = BCrypt.Net.BCrypt.HashPassword(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)), _workFactor);
    }

    public string Hash(string password)
    {
        if (!PasswordRules.IsValidNew(password))
            throw new ArgumentException("Пароль має відповідати правилам нового пароля Application.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
    }

    public bool Verify(string password, string? passwordHash)
    {
        var validInput = PasswordRules.CanVerify(password);
        var validHash = passwordHash is { Length: 60 } && HashFormat.IsMatch(passwordHash);
        try
        {
            var matches = BCrypt.Net.BCrypt.Verify(validInput ? password : "invalid-input", validHash ? passwordHash! : _dummyHash);
            return validInput && validHash && matches;
        }
        catch (Exception error) when (error is BCrypt.Net.SaltParseException or FormatException or ArgumentException)
        {
            return false;
        }
    }
}
