using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(HotelDbContext db) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable();
        return await db.Rooms.Include(x => x.Photos).AsSingleQuery().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    public void Add(Room entity) { db.EnsureUsable(); db.Rooms.Add(entity); }
    public async Task<bool> ExistsByNumberAsync(Guid hotelId, string roomNumber, Guid? excludedRoomId = null, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var number = roomNumber.Trim();
        return await db.Rooms.AnyAsync(x => x.HotelId == hotelId && !x.IsDeleted && x.Id != excludedRoomId && x.RoomNumber == number, cancellationToken);
    }
}
