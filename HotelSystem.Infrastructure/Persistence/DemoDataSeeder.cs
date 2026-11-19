using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Seeding;

public sealed class DemoDataSeeder(
    HotelDbContext db,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwords,
    TimeProvider clock)
{
    public async Task<(int Owners, int Hotels, int Rooms)> SeedAsync(
        CancellationToken cancellationToken = default)
    {
        var ownersCreated = 0;
        var hotelsCreated = 0;
        var roomsCreated = 0;

        // Усі читання виконуються всередині callback: UnitOfWork очищає tracker
        // перед початком транзакції. У кінці callback явно зберігаємо зміни.
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var roles = await db.Roles
                .Where(role => role.Name == RoleNames.User || role.Name == RoleNames.Owner)
                .ToListAsync(ct);
            var userRole = roles.SingleOrDefault(role => role.Name == RoleNames.User);
            var ownerRole = roles.SingleOrDefault(role => role.Name == RoleNames.Owner);

            if (userRole is null || ownerRole is null)
                throw new InvalidOperationException(
                    "Спочатку застосуйте міграції з базовими ролями User та Owner.");

            var now = clock.GetUtcNow();
            var firstOwner = await GetOrCreateOwnerAsync(
                "demo_owner_lviv", "owner1@hotel.test", "Owner1!HotelDemo2026",
                "Галицькі Готелі (демо)", userRole, ownerRole, now, ct);
            var secondOwner = await GetOrCreateOwnerAsync(
                "demo_owner_kyiv", "owner2@hotel.test", "Owner2!HotelDemo2026",
                "Південь Тревел (демо)", userRole, ownerRole, now, ct);

            await AddHotelAsync(firstOwner.Id, "Львівська Брама", "Львів",
                "Городоцька", "25", "Демонстраційний міський готель для подорожей Львовом.", 1600m, now, ct);
            await AddHotelAsync(firstOwner.Id, "Карпатський Затишок", "Яремче",
                "Свободи", "120", "Демонстраційний готель для відпочинку в Карпатах.", 1400m, now, ct);
            await AddHotelAsync(secondOwner.Id, "Дніпровська Тераса", "Київ",
                "Набережно-Хрещатицька", "18", "Демонстраційний готель для відряджень і міського відпочинку.", 2200m, now, ct);
            await AddHotelAsync(secondOwner.Id, "Морський Бриз", "Одеса",
                "Приморська", "40", "Демонстраційний готель для відпочинку біля моря.", 1800m, now, ct);

            await unitOfWork.SaveChangesAsync(ct);

            async Task<User> GetOrCreateOwnerAsync(
                string username, string email, string password, string companyName,
                Role basicRole, Role hotelOwnerRole, DateTimeOffset createdAt, CancellationToken token)
            {
                var emailKey = IdentityNormalization.Key(email);
                var user = await db.Users.Include(account => account.Roles)
                    .SingleOrDefaultAsync(account =>
                        EF.Property<string>(account, "NormalizedEmail") == emailKey, token);

                if (user is not null)
                {
                    // Не підвищуємо довільний наявний акаунт до Owner
                    // та не відновлюємо відкликані ролі чи заблокований акаунт.
                    if (user.Username != username || user.IsBlocked
                        || !user.HasRole(RoleNames.User) || !user.HasRole(RoleNames.Owner)
                        || !await db.OwnerProfiles.AnyAsync(profile => profile.UserId == user.Id, token))
                    {
                        throw new ConflictException(
                            $"Демо-email {email} вже зайнятий акаунтом з іншими налаштуваннями. Наявний акаунт не змінено.");
                    }

                    return user;
                }

                var usernameKey = IdentityNormalization.Key(username);
                if (await db.Users.AnyAsync(account =>
                    EF.Property<string>(account, "NormalizedUsername") == usernameKey, token))
                    throw new ConflictException($"Демо-ім'я {username} вже зайняте. Наявний акаунт не змінено.");

                user = User.Create(username, email, passwords.Hash(password), basicRole, createdAt);
                user.GrantRole(hotelOwnerRole);
                db.Users.Add(user);
                db.OwnerProfiles.Add(OwnerProfile.Create(user.Id, companyName));
                ownersCreated++;
                return user;
            }

            async Task AddHotelAsync(
                Guid ownerId, string name, string city, string street, string number,
                string description, decimal basePrice, DateTimeOffset createdAt, CancellationToken token)
            {
                var hotel = await db.Hotels.SingleOrDefaultAsync(
                    item => item.OwnerId == ownerId && item.Name == name, token);

                if (hotel is null)
                {
                    hotel = Hotel.Create(ownerId, name, Address.Create(city, street, number), description, createdAt);
                    // Це підготовка демо-даних. У звичайному сценарії схвалює адміністратор.
                    hotel.Approve();
                    db.Hotels.Add(hotel);
                    hotelsCreated++;
                }

                // Не відновлюємо видалений готель і не змінюємо його модерацію.
                if (hotel.IsDeleted) return;

                var existingNumbers = await db.Rooms
                    .Where(room => room.HotelId == hotel.Id)
                    .Select(room => room.RoomNumber)
                    .ToListAsync(token);
                var numbers = existingNumbers.ToHashSet(StringComparer.OrdinalIgnoreCase);

                (string Number, RoomType Type, decimal Price, int Capacity)[] roomSeeds =
                [
                    ("101", RoomType.Single, basePrice * 0.75m, 1),
                    ("102", RoomType.Double, basePrice, 2),
                    ("103", RoomType.Twin, basePrice + 100m, 2),
                    ("201", RoomType.Suite, basePrice * 1.6m, 4),
                    ("301", RoomType.Family, basePrice * 1.4m, 5)
                ];

                foreach (var seed in roomSeeds)
                {
                    // Видалені номери також враховані: явно видалені дані не відновлюються.
                    if (!numbers.Add(seed.Number)) continue;
                    db.Rooms.Add(Room.Create(hotel.Id, seed.Number, seed.Type,
                        seed.Price, seed.Capacity, createdAt));
                    roomsCreated++;
                }
            }
        }, cancellationToken);

        return (ownersCreated, hotelsCreated, roomsCreated);
    }
}
