using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Domain.Entities;

public sealed class Booking
{
    private const decimal MaximumTotalPrice =
        BookingRules.MaximumTotalPrice;

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid RoomId { get; private set; }

    public DateOnly CheckIn { get; private set; }
    public DateOnly CheckOut { get; private set; }

    public decimal PricePerNight { get; private set; }
    public decimal TotalPrice { get; private set; }

    public BookingStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    private Booking() { }

    public static Booking Create(
        Guid userId,
        Room room,
        DateOnly checkIn,
        DateOnly checkOut,
        DateOnly currentDate,
        DateTimeOffset createdAt)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(userId),
                "Вкажіть користувача, який бронює номер.");
        }

        EnsureRoomIsAvailable(room);

        var totalPrice = CalculateTotalPrice(
            room.CostPerNight,
            checkIn,
            checkOut,
            currentDate);

        return new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoomId = room.Id,
            CheckIn = checkIn,
            CheckOut = checkOut,
            PricePerNight = room.CostPerNight,
            TotalPrice = totalPrice,
            Status = BookingStatus.Confirmed,
            CreatedAt = createdAt
        };
    }

    public void UpdateDates(
        Room room,
        DateOnly checkIn,
        DateOnly checkOut,
        DateOnly currentDate)
    {
        if (Status == BookingStatus.Cancelled)
        {
            throw new DomainValidationException(
                nameof(Status),
                "Скасоване бронювання не можна редагувати.");
        }

        if (room is null || room.Id != RoomId)
        {
            throw new DomainValidationException(
                nameof(room),
                "Передано інший номер для цього бронювання.");
        }

        if (CheckIn == checkIn && CheckOut == checkOut)
            return;

        EnsureRoomIsAvailable(room);

        var totalPrice = CalculateTotalPrice(
            room.CostPerNight,
            checkIn,
            checkOut,
            currentDate);

        CheckIn = checkIn;
        CheckOut = checkOut;
        PricePerNight = room.CostPerNight;
        TotalPrice = totalPrice;
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return;

        Status = BookingStatus.Cancelled;
    }

    private static void EnsureRoomIsAvailable(Room room)
    {
        if (room is null)
        {
            throw new DomainValidationException(
                nameof(room),
                "Вкажіть номер готелю.");
        }

        if (room.IsDeleted || !room.IsAvailable)
        {
            throw new DomainValidationException(
                nameof(room),
                "Номер тимчасово закритий для бронювання.");
        }
    }

    private static decimal CalculateTotalPrice(
        decimal pricePerNight,
        DateOnly checkIn,
        DateOnly checkOut,
        DateOnly currentDate)
    {
        BookingRules.EnsureDates(checkIn, checkOut, currentDate);

        var nights = checkOut.DayNumber - checkIn.DayNumber;
        var totalPrice = pricePerNight * nights;

        if (totalPrice > MaximumTotalPrice)
        {
            throw new DomainValidationException(
                nameof(TotalPrice),
                "Загальна вартість перевищує підтримуваний діапазон.");
        }

        return totalPrice;
    }
}
