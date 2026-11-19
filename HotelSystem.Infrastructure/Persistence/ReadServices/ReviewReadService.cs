using HotelSystem.Infrastructure.DTOs.Reviews;
using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.Models.Reviews;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class ReviewReadService(HotelDbContext db, IMapper mapper) : IReviewReadService
{
    private IQueryable<ReviewQueryModel> Base()
    {
        db.EnsureUsable();
        return from review in db.Reviews.AsNoTracking()
               join hotel in db.Hotels.AsNoTracking() on review.HotelId equals hotel.Id
               join user in db.Users.AsNoTracking() on review.UserId equals user.Id
               select new ReviewQueryModel { Review = review, Hotel = hotel, User = user };
    }
    private IQueryable<ReviewQueryModel> Public() => Base().Where(x => !x.Review.IsDeleted && !x.Hotel.IsDeleted && x.Hotel.ModerationStatus == HotelModerationStatus.Approved);
    private static IQueryable<ReviewQueryModel> Filter(IQueryable<ReviewQueryModel> q, ReviewFilter filter)
    {
        var f = filter.Normalize();
        if (f.HotelId is { } hotel) q = q.Where(x => x.Hotel.Id == hotel);
        if (f.MinRating is { } min) q = q.Where(x => x.Review.Rating >= min);
        if (f.MaxRating is { } max) q = q.Where(x => x.Review.Rating <= max);
        if (f.Search is { } text)
        {
            var pattern = ReadQuery.Pattern(text);
            q = q.Where(x => EF.Functions.Like(EF.Functions.Collate(x.Review.Comment, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape));
        }
        return q;
    }
    private static IOrderedQueryable<ReviewQueryModel> Sort(IQueryable<ReviewQueryModel> q, ReviewFilter f) =>
        (f.SortBy == ReviewSortBy.Rating
            ? (f.Descending ? q.OrderByDescending(x => x.Review.Rating) : q.OrderBy(x => x.Review.Rating))
            : (f.Descending ? q.OrderByDescending(x => x.Review.CreatedAt) : q.OrderBy(x => x.Review.CreatedAt))).ThenBy(x => x.Review.Id);
    private static IQueryable<ReviewReadData> Project(IQueryable<ReviewQueryModel> q) => q.Select(x => new ReviewReadData(
        x.Review.Id, x.Hotel.Id, x.Hotel.Name, x.User.Id, x.User.Username, x.Review.Rating, x.Review.Comment,
        x.Review.CreatedAt, x.Review.IsDeleted, x.Review.DeletedAt, x.Review.DeletedByUserId, x.Review.Version,
        x.User.IsBlocked, x.Hotel.IsDeleted, x.Hotel.ModerationStatus,
        x.Review.Photos.OrderBy(p => p.Id).Select(p => new PhotoReadData(p.Id, p.StorageKey)).ToList()));
    private Task<PagedResult<TDto>> List<TDto>(IQueryable<ReviewQueryModel> q, ReviewFilter f, CancellationToken ct) =>
        ReadQuery.PageAsync<ReviewQueryModel, ReviewReadData, TDto>(Sort(Filter(q, f), f), f.Page, f.PageSize, Project, mapper, ct);
    private async Task<TDto?> One<TDto>(IQueryable<ReviewQueryModel> q, CancellationToken ct) where TDto : class
    {
        var row = await Project(q).SingleOrDefaultAsync(ct); return row is null ? null : mapper.Map<TDto>(row);
    }
    public Task<PagedResult<ReviewDto>> GetPublicHotelListAsync(Guid hotelId, ReviewFilter filter, CancellationToken ct = default) =>
        List<ReviewDto>(Public().Where(x => x.Hotel.Id == hotelId), filter, ct);
    public Task<ReviewDto?> GetPublicByIdAsync(Guid id, CancellationToken ct = default) => One<ReviewDto>(Public().Where(x => x.Review.Id == id), ct);
    public async Task<HotelReviewSummaryDto> GetPublicHotelSummaryAsync(Guid hotelId, CancellationToken ct = default)
    {
        var row = await Public().Where(x => x.Hotel.Id == hotelId).GroupBy(x => 1)
            .Select(g => new { Count = g.LongCount(), Average = g.Average(x => (decimal)x.Review.Rating) }).SingleOrDefaultAsync(ct);
        return new(hotelId, row?.Count ?? 0, row?.Average);
    }
    public Task<PagedResult<ReviewDto>> GetClientListAsync(Guid userId, ReviewFilter filter, CancellationToken ct = default) =>
        List<ReviewDto>(Base().Where(x => !x.Review.IsDeleted && x.Review.UserId == userId), filter, ct);
    public Task<ReviewDto?> GetClientByIdAsync(Guid userId, Guid id, CancellationToken ct = default) =>
        One<ReviewDto>(Base().Where(x => !x.Review.IsDeleted && x.Review.UserId == userId && x.Review.Id == id), ct);
    public Task<PagedResult<ReviewDto>> GetOwnerListAsync(Guid ownerId, ReviewFilter filter, CancellationToken ct = default) =>
        List<ReviewDto>(Base().Where(x => !x.Review.IsDeleted && x.Hotel.OwnerId == ownerId), filter, ct);
    public Task<ReviewDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken ct = default) =>
        One<ReviewDto>(Base().Where(x => !x.Review.IsDeleted && x.Hotel.OwnerId == ownerId && x.Review.Id == id), ct);
    public Task<PagedResult<ReviewModerationDto>> GetModerationListAsync(ReviewFilter filter, Guid? userId, bool includeDeleted, CancellationToken ct = default)
    {
        var q = Base(); if (!includeDeleted) q = q.Where(x => !x.Review.IsDeleted);
        if (userId is { } user) q = q.Where(x => x.User.Id == user);
        return List<ReviewModerationDto>(q, filter, ct);
    }
    public Task<ReviewModerationDto?> GetForModerationByIdAsync(Guid id, CancellationToken ct = default) =>
        One<ReviewModerationDto>(Base().Where(x => x.Review.Id == id), ct);
}
