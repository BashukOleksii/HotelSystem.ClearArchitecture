using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Entities;

public sealed class Photo
{
    public Guid Id { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;

    private Photo() { }

    internal static Photo Create(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new DomainValidationException(
                nameof(storageKey),
                "Вкажіть ключ збереженого файлу.");
        }

        return new Photo
        {
            Id = Guid.NewGuid(),
            StorageKey = storageKey
        };
    }
}