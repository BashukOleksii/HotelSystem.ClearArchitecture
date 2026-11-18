using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Rules;
using HotelSystem.Domain.Services;

namespace HotelSystem.Domain.Entities;

public sealed class Room
{
    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public string RoomNumber { get; private set; } = string.Empty;
    public RoomType Type { get; private set; }
    public decimal CostPerNight { get; private set; }
    public int Capacity { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid Version { get; private set; }
    private readonly List<Photo> _photos = [];
    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();
    private Room()
    {
    }
    public static Room Create(Guid hotelId, string roomNumber, RoomType type,
        decimal costPerNight, int capacity, DateTimeOffset createdAt, bool isAvailable = true)
    {
        if (hotelId == Guid.Empty)
            throw new DomainValidationException(nameof(hotelId), "Вкажіть ідентифікатор готелю.");
        var room = new Room { Id = Guid.NewGuid(), HotelId = hotelId, CreatedAt = createdAt };
        room.UpdateDetails(roomNumber, type, costPerNight, capacity, isAvailable);
        return room;
    }
    public void UpdateDetails(string roomNumber, RoomType type, decimal costPerNight,
        int capacity, bool isAvailable)
    {
        EnsureNotDeleted();
        var number = roomNumber?.Trim();
        if (string.IsNullOrEmpty(number) || number.Length > RoomRules.MaximumNumberLength)
            throw new DomainValidationException(nameof(roomNumber), "Номер кімнати має містити від 1 до 20 символів.");
        EnsureCapacity(type, capacity);
        EnsurePrice(costPerNight);
        if (RoomNumber == number && Type == type && CostPerNight == costPerNight
            && Capacity == capacity && IsAvailable == isAvailable)
            return;
        RoomNumber = number;
        Type = type;
        CostPerNight = costPerNight;
        Capacity = capacity;
        IsAvailable = isAvailable;
        Touch();
    }
    public void ChangePrice(decimal costPerNight)
    {
        EnsureNotDeleted();
        EnsurePrice(costPerNight);
        if (CostPerNight == costPerNight)
            return;
        CostPerNight = costPerNight;
        Touch();
    }
    public void ChangeCapacity(int capacity)
    {
        EnsureNotDeleted();
        EnsureCapacity(Type, capacity);
        if (Capacity == capacity)
            return;
        Capacity = capacity;
        Touch();
    }
    public void OpenForBooking()
    {
        EnsureNotDeleted();
        if (IsAvailable)
            return;
        IsAvailable = true;
        Touch();
    }
    public void CloseForBooking()
    {
        EnsureNotDeleted();
        if (!IsAvailable)
            return;
        IsAvailable = false;
        Touch();
    }
    public void Delete()
    {
        if (IsDeleted)
            return;
        IsDeleted = true;
        IsAvailable = false;
        Touch();
    }
    public Photo AddPhoto(string storageKey)
    {
        EnsureNotDeleted();
        var photo = PhotoGalleryService.Add(_photos, storageKey, RoomRules.MaximumPhotoCount);
        Touch();
        return photo;
    }
    public Photo RemovePhoto(Guid photoId)
    {
        EnsureNotDeleted();
        var photo = PhotoGalleryService.Remove(_photos, photoId);
        Touch();
        return photo;
    }
    private void Touch() => Version = Guid.NewGuid();
    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainValidationException(nameof(IsDeleted), "Видалений номер не можна змінювати.");
    }
    private static void EnsurePrice(decimal price)
    {
        if (price <= 0 || price > RoomRules.MaximumPrice || decimal.Round(price, 2) != price)
            throw new DomainValidationException(nameof(CostPerNight), "Ціна має бути додатною та відповідати decimal(18,2) без округлення.");
    }
    private static void EnsureCapacity(RoomType type, int capacity)
    {
        var max = RoomRules.MaximumCapacity(type);
        if (capacity < 1 || capacity > max)
            throw new DomainValidationException(nameof(Capacity), $"Для {type} допустима місткість від 1 до {max} гостей.");
    }
}
