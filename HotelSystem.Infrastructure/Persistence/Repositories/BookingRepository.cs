using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(HotelDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); return await db.Bookings.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public async Task<bool> HasOverlapAsync(Guid roomId, DateOnly checkIn, DateOnly checkOut, Guid? excludedBookingId = null, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Bookings.AnyAsync(x => x.RoomId == roomId && x.Status == HotelSystem.Domain.Enums.BookingStatus.Confirmed
            && x.Id != excludedBookingId && x.CheckIn < checkOut && x.CheckOut > checkIn, cancellationToken);
    }
    public async Task<bool> HasActiveForRoomAsync(Guid roomId, DateOnly currentDate, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Bookings.AnyAsync(x => x.RoomId == roomId && x.Status == HotelSystem.Domain.Enums.BookingStatus.Confirmed && x.CheckOut > currentDate, cancellationToken);
    }
    public void Add(Booking entity) { db.EnsureUsable(); db.Bookings.Add(entity); }
}
