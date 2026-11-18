using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Role() { }

    public static Role Create(string name)
    {
        var normalizedName = name?.Trim();

        if (string.IsNullOrEmpty(normalizedName)
            || normalizedName.Length > 50)
        {
            throw new DomainValidationException(
                nameof(name),
                "Назва ролі має містити від 1 до 50 символів.");
        }

        return new Role
        {
            Id = Guid.NewGuid(),
            Name = normalizedName
        };
    }
}