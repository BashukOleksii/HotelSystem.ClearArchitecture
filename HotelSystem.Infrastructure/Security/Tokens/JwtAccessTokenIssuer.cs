using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelSystem.Infrastructure.Security.Tokens;

public sealed class JwtAccessTokenIssuer : IAccessTokenIssuer
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _minutes;
    private readonly SigningCredentials _credentials;
    private readonly TimeProvider _clock;

    public JwtAccessTokenIssuer(IOptions<JwtOptions> options, TimeProvider clock)
    {
        var config = options.Value;
        var result = new JwtOptionsValidator().Validate(Options.DefaultName, config);
        if (result.Failed) throw new OptionsValidationException(Options.DefaultName, typeof(JwtOptions), result.Failures);

        _issuer = config.Issuer;
        _audience = config.Audience;
        _minutes = config.AccessTokenMinutes;
        _credentials = new(new SymmetricSecurityKey(Convert.FromBase64String(config.SigningKeyBase64)), SecurityAlgorithms.HmacSha256);
        _clock = clock;
    }

    public Task<AccessTokenDto> IssueAsync(Guid userId, string username, IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (userId == Guid.Empty) throw new ArgumentException("Вкажіть користувача.", nameof(userId));
        if (string.IsNullOrWhiteSpace(username) || username.Length > 50 || username != username.Trim())
            throw new ArgumentException("Некоректне ім'я користувача.", nameof(username));
        ArgumentNullException.ThrowIfNull(roles);
        if (roles.Count is < 1 or > 32 || !roles.Contains(RoleNames.User, StringComparer.Ordinal)
            || roles.Any(x => string.IsNullOrWhiteSpace(x) || x.Length > 50 || x != x.Trim() || x.Any(char.IsControl)))
            throw new ArgumentException("Потрібна базова роль User та коректні назви ролей.", nameof(roles));

        var now = DateTimeOffset.FromUnixTimeSeconds(_clock.GetUtcNow().ToUnixTimeSeconds());
        var expiresAt = now.AddMinutes(_minutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString("D")),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        claims.AddRange(roles.Distinct(StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).Select(x => new Claim("role", x)));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _issuer,
            Audience = _audience,
            IssuedAt = now.UtcDateTime,
            NotBefore = now.UtcDateTime,
            Expires = expiresAt.UtcDateTime,
            SigningCredentials = _credentials,
            TokenType = "at+jwt"
        };
        var token = new JwtSecurityTokenHandler().CreateEncodedJwt(descriptor);
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new AccessTokenDto(token, expiresAt));
    }
}
