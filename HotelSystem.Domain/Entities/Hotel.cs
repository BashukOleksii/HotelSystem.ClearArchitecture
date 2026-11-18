using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Services;
using HotelSystem.Domain.ValueObjects;

namespace HotelSystem.Domain.Entities;

public sealed class Hotel
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public Address Address { get; private set; } = null!;

    public HotelModerationStatus ModerationStatus { get; private set; }
    public bool IsDeleted { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    private const int MaximumPhotoCount = 10;

    private readonly List<Photo> _photos = [];

    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    private Hotel() { }

    public static Hotel Create(
        Guid ownerId,
        string name,
        Address address,
        string? description,
        DateTimeOffset createdAt)
    {
        if (ownerId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(ownerId),
                "Вкажіть власника готелю.");
        }

        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            CreatedAt = createdAt
        };

        hotel.UpdateDetails(
            name,
            address,
            description);

        return hotel;
    }

    public void UpdateDetails(
        string name,
        Address address,
        string? description)
    {
        EnsureNotDeleted();

        var normalizedName = name?.Trim();

        if (string.IsNullOrEmpty(normalizedName)
            || normalizedName.Length > 100)
        {
            throw new DomainValidationException(
                nameof(name),
                "Назва має містити від 1 до 100 символів.");
        }

        if (address is null)
        {
            throw new DomainValidationException(
                nameof(address),
                "Вкажіть адресу готелю.");
        }

        var normalizedDescription = description?.Trim();

        if (normalizedDescription?.Length > 2000)
        {
            throw new DomainValidationException(
                nameof(description),
                "Опис має містити до 2000 символів.");
        }

        if (normalizedDescription is "")
        {
            normalizedDescription = null;
        }

        if (Name == normalizedName
            && Address == address
            && Description == normalizedDescription)
        {
            return;
        }

        Name = normalizedName;
        Address = address;
        Description = normalizedDescription;

        ModerationStatus = HotelModerationStatus.Pending;
    }

    public void Approve()
    {
        EnsureNotDeleted();

        ModerationStatus = HotelModerationStatus.Approved;
    }

    public void Reject()
    {
        EnsureNotDeleted();

        ModerationStatus = HotelModerationStatus.Rejected;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public Photo AddPhoto(string storageKey)
    {
        EnsureNotDeleted();

        var photo = PhotoGalleryService.Add(
            _photos,
            storageKey,
            MaximumPhotoCount);

        ModerationStatus = HotelModerationStatus.Pending;

        return photo;
    }

    public Photo RemovePhoto(Guid photoId)
    {
        EnsureNotDeleted();

        var photo = PhotoGalleryService.Remove(
            _photos,
            photoId);

        ModerationStatus = HotelModerationStatus.Pending;

        return photo;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
        {
            throw new DomainValidationException(
                nameof(IsDeleted),
                "Видалений готель не можна змінювати.");
        }
    }
}