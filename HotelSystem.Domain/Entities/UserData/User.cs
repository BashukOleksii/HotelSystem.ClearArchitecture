using System.Net.Mail;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Entities;

public sealed class User
{
    private readonly List<Role> _roles = [];

    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsBlocked { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    private User() { }

    public static User Create(
        string username,
        string email,
        string passwordHash,
        Role userRole,
        DateTimeOffset createdAt)
    {
        if (userRole is null || userRole.Name != RoleNames.User)
        {
            throw new DomainValidationException(
                nameof(userRole),
                "Для створення акаунта потрібна базова роль User.");
        }

        var normalizedUsername = NormalizeUsername(username);
        var normalizedEmail = NormalizeEmail(email);

        EnsurePasswordHash(passwordHash);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = normalizedUsername,
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            CreatedAt = createdAt
        };

        user._roles.Add(userRole);

        return user;
    }

    public void UpdateProfile(string username, string email)
    {
        var normalizedUsername = NormalizeUsername(username);
        var normalizedEmail = NormalizeEmail(email);

        Username = normalizedUsername;
        Email = normalizedEmail;
    }

    public bool HasRole(string roleName)
    {
        return _roles.Any(role => role.Name == roleName);
    }

    public void GrantRole(Role role)
    {
        if (role is null)
        {
            throw new DomainValidationException(
                nameof(role),
                "Вкажіть роль для призначення.");
        }

        if (_roles.Any(existing =>
            existing.Id == role.Id || existing.Name == role.Name))
        {
            return;
        }

        _roles.Add(role);
    }

    public void RevokeRole(Guid roleId)
    {
        var role = _roles.FirstOrDefault(
            existing => existing.Id == roleId);

        if (role is null)
            return;

        if (role.Name == RoleNames.User)
        {
            throw new DomainValidationException(
                nameof(roleId),
                "Базову роль User відкликати не можна.");
        }

        _roles.Remove(role);
    }

    public void ChangePasswordHash(string passwordHash)
    {
        EnsurePasswordHash(passwordHash);
        PasswordHash = passwordHash;
    }

    public void Block() => IsBlocked = true;

    public void Unblock() => IsBlocked = false;

    private static string NormalizeUsername(string username)
    {
        var normalized = username?.Trim();

        if (string.IsNullOrEmpty(normalized)
            || normalized.Length < 3
            || normalized.Length > 50)
        {
            throw new DomainValidationException(
                nameof(username),
                "Ім'я користувача має містити від 3 до 50 символів.");
        }

        return normalized;
    }

    private static string NormalizeEmail(string email)
    {
        var normalized = email?.Trim();

        if (string.IsNullOrEmpty(normalized)
            || normalized.Length > 254
            || !MailAddress.TryCreate(normalized, out var address)
            || address.Address != normalized)
        {
            throw new DomainValidationException(
                nameof(email),
                "Вкажіть коректну адресу електронної пошти до 254 символів.");
        }

        return normalized;
    }

    private static void EnsurePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainValidationException(
                nameof(passwordHash),
                "Хеш пароля є обов'язковим.");
        }
    }
}