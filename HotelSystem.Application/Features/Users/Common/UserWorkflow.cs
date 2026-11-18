using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;

namespace HotelSystem.Application.Features.Users.Common;

internal static class UserWorkflow
{
    public static async Task<User> RequireAsync(Guid actorId, string role, IUserRepository users, CancellationToken ct)
    {
        var user = await users.GetByIdAsync(actorId, ct);
        if (user is null || user.IsBlocked || !user.HasRole(role))
            throw new ForbiddenOperationException($"Потрібен активний користувач із роллю {role}.");
        return user;
    }

    public static async Task<User> TargetAsync(Guid id, IUserRepository users, CancellationToken ct) =>
        await users.GetByIdAsync(id, ct) ?? throw new NotFoundException("Користувача не знайдено.");

    public static async Task EnsureUniqueAsync(UserDetailsInput input, Guid? except, IUserRepository users, CancellationToken ct)
    {
        var usernameExists = await users.ExistsByUsernameAsync(input.Username.Trim(), except, ct);
        var emailExists = await users.ExistsByEmailAsync(input.Email.Trim(), except, ct);
        if (usernameExists || emailExists)
            throw new ConflictException("Не вдалося використати ці облікові дані. Оберіть інші.");
    }

    public static async Task<UserAccountDto> MapAsync(User user, IUserProfileRepository profiles, CancellationToken ct)
    {
        var owner = await profiles.GetOwnerAsync(user.Id, ct);
        var admin = await profiles.GetAdminAsync(user.Id, ct);
        return new(user.Id, user.Username, user.Email, user.PhoneNumber, user.IsBlocked, user.CreatedAt,
            user.Roles.Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal).ToArray(),
            owner is null ? null : new(owner.CompanyName, owner.BusinessPhone),
            admin is null ? null : new(admin.EmployeeNumber, admin.DepartmentName));
    }
}
