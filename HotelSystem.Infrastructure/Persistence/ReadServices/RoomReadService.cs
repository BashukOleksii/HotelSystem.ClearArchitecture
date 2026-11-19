using HotelSystem.Infrastructure.DTOs.Rooms;
using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.Models.Rooms;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Application.Features.Rooms.Queries;
using HotelSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class RoomReadService(HotelDbContext db, IMapper mapper) : IRoomReadService
{
    private IQueryable<RoomQueryModel> Base()
    {
        db.EnsureUsable();
        return from room in db.Rooms.AsNoTracking()
               join hotel in db.Hotels.AsNoTracking() on room.HotelId equals hotel.Id
               where !room.IsDeleted && !hotel.IsDeleted
               select new RoomQueryModel { Room = room, Hotel = hotel };
    }
    private IQueryable<RoomQueryModel> Filter(IQueryable<RoomQueryModel> q, RoomFilter filter)
    {
        var f = filter.Normalize();
        if (f.HotelId is { } hotel) q = q.Where(x => x.Room.HotelId == hotel);
        if (f.Search is { } text)
        {
            var pattern = ReadQuery.Pattern(text);
            q = q.Where(x => EF.Functions.Like(EF.Functions.Collate(x.Room.RoomNumber, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape)
                || EF.Functions.Like(EF.Functions.Collate(x.Hotel.Name, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape));
        }
        if (f.City is { } city) q = q.Where(x => EF.Functions.Collate(x.Hotel.Address.City, ReadQuery.CaseInsensitiveCollation) == city);
        if (f.Type is { } type) q = q.Where(x => x.Room.Type == type);
        if (f.MinPrice is { } min) q = q.Where(x => x.Room.CostPerNight >= min);
        if (f.MaxPrice is { } max) q = q.Where(x => x.Room.CostPerNight <= max);
        if (f.MinCapacity is { } capacity) q = q.Where(x => x.Room.Capacity >= capacity);
        if (f.IsAvailable is { } open) q = q.Where(x => x.Room.IsAvailable == open);
        if (f.Stay is { } stay)
        {
            var from = stay.CheckIn; var to = stay.CheckOut;
            q = q.Where(x => x.Room.IsAvailable && !db.Bookings.Any(b => b.RoomId == x.Room.Id
                && b.Status == BookingStatus.Confirmed && b.CheckIn < to && b.CheckOut > from));
        }
        return q;
    }
    private static IOrderedQueryable<RoomQueryModel> Sort(IQueryable<RoomQueryModel> q, RoomFilter f)
    {
        IOrderedQueryable<RoomQueryModel> ordered = f.SortBy switch
        {
            RoomSortBy.Price => f.Descending ? q.OrderByDescending(x => x.Room.CostPerNight) : q.OrderBy(x => x.Room.CostPerNight),
            RoomSortBy.Capacity => f.Descending ? q.OrderByDescending(x => x.Room.Capacity) : q.OrderBy(x => x.Room.Capacity),
            RoomSortBy.Type => f.Descending ? q.OrderByDescending(x => x.Room.Type) : q.OrderBy(x => x.Room.Type),
            RoomSortBy.CreatedAt => f.Descending ? q.OrderByDescending(x => x.Room.CreatedAt) : q.OrderBy(x => x.Room.CreatedAt),
            _ => f.Descending ? q.OrderByDescending(x => x.Room.RoomNumber) : q.OrderBy(x => x.Room.RoomNumber)
        };
        return ordered.ThenBy(x => x.Room.Id);
    }
    private IQueryable<RoomReadData> Project(IQueryable<RoomQueryModel> q, RoomStay? stay = null)
    {
        DateOnly from = stay?.CheckIn ?? default, to = stay?.CheckOut ?? default; var supplied = stay is not null;
        return q.Select(x => new RoomReadData(x.Room.Id, x.Room.HotelId, x.Hotel.OwnerId, x.Hotel.Name,
            x.Hotel.ModerationStatus, x.Hotel.Address.City, x.Hotel.Address.Street, x.Hotel.Address.Number,
            x.Room.RoomNumber, x.Room.Type, x.Room.CostPerNight, x.Room.Capacity, x.Room.IsAvailable,
            x.Room.CreatedAt, x.Room.Version,
            supplied ? (bool?)(x.Room.IsAvailable && !db.Bookings.Any(b => b.RoomId == x.Room.Id && b.Status == BookingStatus.Confirmed && b.CheckIn < to && b.CheckOut > from)) : null,
            x.Room.Photos.OrderBy(p => p.Id).Select(p => new PhotoReadData(p.Id, p.StorageKey)).ToList()));
    }
    private Task<PagedResult<TDto>> List<TDto>(IQueryable<RoomQueryModel> q, RoomFilter f, CancellationToken ct) =>
        ReadQuery.PageAsync<RoomQueryModel, RoomReadData, TDto>(Sort(Filter(q, f), f), f.Page, f.PageSize, x => Project(x), mapper, ct);
    private async Task<TDto?> One<TDto>(IQueryable<RoomQueryModel> q, RoomStay? stay, CancellationToken ct) where TDto : class
    {
        var row = await Project(q, stay).SingleOrDefaultAsync(ct); return row is null ? null : mapper.Map<TDto>(row);
    }
    public Task<PagedResult<RoomListItemDto>> SearchPublicAsync(RoomFilter filter, CancellationToken ct = default) =>
        List<RoomListItemDto>(Base().Where(x => x.Hotel.ModerationStatus == HotelModerationStatus.Approved), filter, ct);
    public Task<RoomDetailsDto?> GetPublicByIdAsync(Guid id, RoomStay? stay, CancellationToken ct = default) =>
        One<RoomDetailsDto>(Base().Where(x => x.Room.Id == id && x.Hotel.ModerationStatus == HotelModerationStatus.Approved), stay, ct);
    public Task<PagedResult<RoomManagementDto>> GetOwnerListAsync(Guid ownerId, RoomFilter filter, CancellationToken ct = default) =>
        List<RoomManagementDto>(Base().Where(x => x.Hotel.OwnerId == ownerId), filter, ct);
    public Task<RoomManagementDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken ct = default) =>
        One<RoomManagementDto>(Base().Where(x => x.Hotel.OwnerId == ownerId && x.Room.Id == id), null, ct);
    public Task<PagedResult<RoomManagementDto>> GetModerationListAsync(RoomFilter filter, CancellationToken ct = default) => List<RoomManagementDto>(Base(), filter, ct);
    public Task<RoomManagementDto?> GetForModerationByIdAsync(Guid id, CancellationToken ct = default) =>
        One<RoomManagementDto>(Base().Where(x => x.Room.Id == id), null, ct);
}
