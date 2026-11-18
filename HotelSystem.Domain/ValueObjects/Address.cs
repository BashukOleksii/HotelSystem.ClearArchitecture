using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.ValueObjects;

public sealed record Address
{
    public string City { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;

    private Address() { }

    public static Address Create(
        string city,
        string street,
        string number)
    {
        return new Address
        {
            City = Normalize(city, 100, nameof(city)),
            Street = Normalize(street, 100, nameof(street)),
            Number = Normalize(number, 20, nameof(number))
        };
    }

    private static string Normalize(
        string value,
        int maximumLength,
        string field)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrEmpty(normalized)
            || normalized.Length > maximumLength)
        {
            throw new DomainValidationException(
                field,
                $"Поле має містити від 1 до {maximumLength} символів.");
        }

        return normalized;
    }
}