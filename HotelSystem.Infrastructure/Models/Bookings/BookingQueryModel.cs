using HotelSystem.Domain.Entities;

namespace HotelSystem.Infrastructure.Models.Bookings;

internal sealed class BookingQueryModel
{
    public Booking Booking { get; init; } = null!;
    public Room Room { get; init; } = null!;
    public Hotel Hotel { get; init; } = null!;
    public User User { get; init; } = null!;
}
