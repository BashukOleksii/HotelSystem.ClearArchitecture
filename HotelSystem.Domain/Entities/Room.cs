using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Services;

namespace HotelSystem.Domain.Entities;

public sealed class Room
{
    private const decimal MaximumPrice = 9_999_999_999_999_999.99m;

    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public string RoomNumber { get; private set; } = string.Empty;
    public RoomType Type { get; private set; }
    public decimal CostPerNight { get; private set; }
    public int Capacity { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private const int MaximumPhotoCount = 10;
    private readonly List<Photo> _photos = [];
    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    private Room() { }

    public static Room Create(
        Guid hotelId, string roomNumber, RoomType type, decimal costPerNight,
        int capacity, DateTimeOffset createdAt, bool isAvailable = true)
    {
        if (hotelId == Guid.Empty)
            throw new DomainValidationException(nameof(hotelId), "Вкажіть ідентифікатор готелю.");

        var room = new Room
        {
            Id = Guid.NewGuid(),
            HotelId = hotelId,
            CreatedAt = createdAt
        };

        room.UpdateDetails(roomNumber, type, costPerNight, capacity, isAvailable);

        return room;
    }

    public void UpdateDetails(
        string roomNumber, RoomType type, decimal costPerNight,
        int capacity, bool isAvailable)
    {
        var normalizedNumber = roomNumber?.Trim();
        if (string.IsNullOrEmpty(normalizedNumber) || normalizedNumber.Length > 20)
            throw new DomainValidationException(nameof(roomNumber), "Номер кімнати має містити від 1 до 20 символів.");

        EnsureValidCapacity(type, capacity);
        EnsureValidPrice(costPerNight);

        RoomNumber = normalizedNumber;
        Type = type;
        CostPerNight = costPerNight;
        Capacity = capacity;
        IsAvailable = isAvailable;
    }

    public void ChangePrice(decimal costPerNight)
    {
        EnsureValidPrice(costPerNight);
        CostPerNight = costPerNight;
    }

    public void ChangeCapacity(int capacity)
    {
        EnsureValidCapacity(Type, capacity);
        Capacity = capacity;
    }

    public void OpenForBooking() => IsAvailable = true;

    public void CloseForBooking() => IsAvailable = false;

    private static void EnsureValidPrice(decimal costPerNight)
    {
        if (costPerNight <= 0 || costPerNight > MaximumPrice
            || decimal.Round(costPerNight, 2) != costPerNight)
        {
            throw new DomainValidationException(nameof(costPerNight),
                "Ціна за ніч має бути додатною та вміщатися у decimal(18,2) без округлення.");
        }
    }

    private static void EnsureValidCapacity(RoomType type, int capacity)
    {
        var maximumCapacity = type switch
        {
            RoomType.Single => 1,
            RoomType.Double => 2,
            RoomType.Twin => 2,
            RoomType.Suite => 4,
            RoomType.Family => 5,
            _ => throw new DomainValidationException(nameof(type), "Невідомий тип номера.")
        };

        if (capacity < 1 || capacity > maximumCapacity)
        {
            throw new DomainValidationException(nameof(capacity),
                $"Для номера типу {type} допустима місткість від 1 до {maximumCapacity} гостей.");
        }
    }

    public Photo AddPhoto(string storageKey)
    {
        return PhotoGalleryService.Add(
            _photos,
            storageKey,
            MaximumPhotoCount);
    }

    public Photo RemovePhoto(Guid photoId)
    {
        return PhotoGalleryService.Remove(_photos, photoId);
    }
}