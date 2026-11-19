using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Persistence.Seeding;

public sealed class BootstrapAdminSeeder(HotelDbContext db, IUnitOfWork unitOfWork, IPasswordHasher passwords,
    IOptions<BootstrapAdminOptions> options, TimeProvider clock)
{
    public async Task<bool> SeedAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var config = options.Value;
        if (!config.Enabled) return false;
        var validation = new BootstrapAdminOptionsValidator().Validate(Options.DefaultName, config);
        if (validation.Failed)
            throw new OptionsValidationException(Options.DefaultName, typeof(BootstrapAdminOptions), validation.Failures);

        db.EnsureUsable();
        if (db.ChangeTracker.HasChanges()) throw new InvalidOperationException("Запустіть початкове налаштування в окремому чистому scope.");
        if (await HasAnyAdminAsync(cancellationToken)) return false;
        var hash = passwords.Hash(config.Password);
        var created = false;

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            if (await HasAnyAdminAsync(ct)) return;

            var roles = await db.Roles.Where(x => x.Name == RoleNames.User || x.Name == RoleNames.Admin).ToListAsync(ct);
            var userRole = roles.SingleOrDefault(x => x.Name == RoleNames.User);
            var adminRole = roles.SingleOrDefault(x => x.Name == RoleNames.Admin);
            if (userRole is null || adminRole is null)
                throw new InvalidOperationException("Спочатку застосуйте міграції з базовими ролями User та Admin.");

            var emailKey = IdentityNormalization.Key(config.Email);
            var usernameKey = IdentityNormalization.Key(config.Username);
            if (await db.Users.AnyAsync(x => EF.Property<string>(x, "NormalizedEmail") == emailKey
                || EF.Property<string>(x, "NormalizedUsername") == usernameKey, ct))
                throw new ConflictException("Облікові дані початкового адміністратора вже зайняті. Існуючий акаунт не підвищено.");

            var user = User.Create(config.Username, config.Email, hash, userRole, clock.GetUtcNow(), config.PhoneNumber);
            user.GrantRole(adminRole);
            db.Users.Add(user);
            db.AdminProfiles.Add(AdminProfile.Create(user.Id, config.EmployeeNumber, config.DepartmentName));
            await unitOfWork.SaveChangesAsync(ct);
            created = true;
        }, cancellationToken);

        return created;
    }

    private Task<bool> HasAnyAdminAsync(CancellationToken ct) =>
        db.Users.AsNoTracking().AnyAsync(x => x.Roles.Any(r => r.Name == RoleNames.Admin), ct);
}
