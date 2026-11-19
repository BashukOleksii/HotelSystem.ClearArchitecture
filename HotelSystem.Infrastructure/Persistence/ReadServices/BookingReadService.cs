using HotelSystem.Infrastructure.DTOs.Bookings;
using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.Models.Bookings;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Application.Features.Bookings.Queries;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class BookingReadService(HotelDbContext db, IMapper mapper) : IBookingReadService
{
    private IQueryable<BookingQueryModel> Base()
    {
        db.EnsureUsable();
        return from booking in db.Bookings.AsNoTracking()
               join room in db.Rooms.AsNoTracking() on booking.RoomId equals room.Id
               join hotel in db.Hotels.AsNoTracking() on room.HotelId equals hotel.Id
               join user in db.Users.AsNoTracking() on booking.UserId equals user.Id
               select new BookingQueryModel { Booking = booking, Room = room, Hotel = hotel, User = user };
    }
    private static IQueryable<BookingQueryModel> Filter(IQueryable<BookingQueryModel> q, BookingFilter filter)
    {
        var f = filter.Normalize();
        if (f.HotelId is { } hotel) q = q.Where(x => x.Hotel.Id == hotel);
        if (f.RoomId is { } room) q = q.Where(x => x.Room.Id == room);
        if (f.Status is { } status) q = q.Where(x => x.Booking.Status == status);
        if (f.From is { } from) q = q.Where(x => x.Booking.CheckOut >= from);
        if (f.To is { } to) q = q.Where(x => x.Booking.CheckIn <= to);
        if (f.Search is { } text)
        {
            var pattern = ReadQuery.Pattern(text);
            q = q.Where(x => EF.Functions.Like(EF.Functions.Collate(x.Hotel.Name, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape)
                || EF.Functions.Like(EF.Functions.Collate(x.Room.RoomNumber, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape));
        }
        return q;
    }
    private static IOrderedQueryable<BookingQueryModel> Sort(IQueryable<BookingQueryModel> q, BookingFilter f)
    {
        IOrderedQueryable<BookingQueryModel> ordered = f.SortBy switch
        {
            BookingSortBy.CheckIn => f.Descending ? q.OrderByDescending(x => x.Booking.CheckIn) : q.OrderBy(x => x.Booking.CheckIn),
            BookingSortBy.CheckOut => f.Descending ? q.OrderByDescending(x => x.Booking.CheckOut) : q.OrderBy(x => x.Booking.CheckOut),
            BookingSortBy.TotalPrice => f.Descending ? q.OrderByDescending(x => x.Booking.TotalPrice) : q.OrderBy(x => x.Booking.TotalPrice),
            _ => f.Descending ? q.OrderByDescending(x => x.Booking.CreatedAt) : q.OrderBy(x => x.Booking.CreatedAt)
        };
        return ordered.ThenBy(x => x.Booking.Id);
    }
    private static IQueryable<BookingReadData> Project(IQueryable<BookingQueryModel> q) => q.Select(x => new BookingReadData(
        x.Booking.Id, x.Room.Id, x.Hotel.Id, x.Hotel.OwnerId, x.User.Id, x.User.Username, x.User.Email,
        x.Hotel.Name, x.Room.RoomNumber, x.Hotel.Address.City, x.Hotel.Address.Street, x.Hotel.Address.Number,
        x.Booking.CheckIn, x.Booking.CheckOut, x.Booking.PricePerNight, x.Booking.TotalPrice, x.Booking.Status,
        x.Booking.CreatedAt, x.Hotel.IsDeleted, x.Room.IsDeleted, x.Room.IsAvailable, x.Hotel.ModerationStatus,
        x.Room.Photos.OrderBy(p => p.Id).Select(p => new PhotoReadData(p.Id, p.StorageKey)).ToList()));
    private Task<PagedResult<TDto>> List<TDto>(IQueryable<BookingQueryModel> q, BookingFilter f, CancellationToken ct) =>
        ReadQuery.PageAsync<BookingQueryModel, BookingReadData, TDto>(Sort(Filter(q, f), f), f.Page, f.PageSize, Project, mapper, ct);
    private async Task<TDto?> One<TDto>(IQueryable<BookingQueryModel> q, CancellationToken ct) where TDto : class
    {
        var row = await Project(q).SingleOrDefaultAsync(ct); return row is null ? null : mapper.Map<TDto>(row);
    }
    public Task<PagedResult<BookingListItemDto>> GetClientListAsync(Guid userId, BookingFilter filter, CancellationToken ct = default) =>
        List<BookingListItemDto>(Base().Where(x => x.Booking.UserId == userId), filter, ct);
    public Task<BookingDetailsDto?> GetClientByIdAsync(Guid userId, Guid id, CancellationToken ct = default) =>
        One<BookingDetailsDto>(Base().Where(x => x.Booking.UserId == userId && x.Booking.Id == id), ct);
    public Task<PagedResult<BookingManagementDto>> GetOwnerListAsync(Guid ownerId, BookingFilter filter, CancellationToken ct = default) =>
        List<BookingManagementDto>(Base().Where(x => x.Hotel.OwnerId == ownerId), filter, ct);
    public Task<BookingManagementDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken ct = default) =>
        One<BookingManagementDto>(Base().Where(x => x.Hotel.OwnerId == ownerId && x.Booking.Id == id), ct);
}
