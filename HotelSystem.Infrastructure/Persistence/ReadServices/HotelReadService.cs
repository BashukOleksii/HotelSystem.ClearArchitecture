using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.DTOs.Hotels;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Application.Features.Hotels.Queries;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class HotelReadService(HotelDbContext db, IMapper mapper) : IHotelReadService
{
    private IQueryable<Hotel> Base()
    {
        db.EnsureUsable(); return db.Hotels.AsNoTracking().Where(x => !x.IsDeleted);
    }
    private static IQueryable<Hotel> Filter(IQueryable<Hotel> q, string? search, string? city, HotelModerationStatus? status = null)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = ReadQuery.Pattern(search.Trim());
            q = q.Where(x => EF.Functions.Like(EF.Functions.Collate(x.Name, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape)
                || EF.Functions.Like(EF.Functions.Collate(x.Description, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape));
        }
        if (!string.IsNullOrWhiteSpace(city))
        {
            var value = city.Trim(); q = q.Where(x => EF.Functions.Collate(x.Address.City, ReadQuery.CaseInsensitiveCollation) == value);
        }
        if (status is { } s) q = q.Where(x => x.ModerationStatus == s);
        return q;
    }
    private static IQueryable<HotelReadData> Project(IQueryable<Hotel> q) => q.Select(x => new HotelReadData(
        x.Id, x.OwnerId, x.Name, x.Description, x.Address.City, x.Address.Street, x.Address.Number,
        x.ModerationStatus, x.CreatedAt, x.Photos.OrderBy(p => p.Id).Select(p => new PhotoReadData(p.Id, p.StorageKey)).ToList()));

    private Task<PagedResult<TDto>> List<TDto>(IQueryable<Hotel> q, int page, int size, CancellationToken ct) =>
        ReadQuery.PageAsync<Hotel, HotelReadData, TDto>(q.OrderBy(x => x.Name).ThenBy(x => x.Id), page, size, Project, mapper, ct);
    private async Task<TDto?> One<TDto>(IQueryable<Hotel> q, CancellationToken ct) where TDto : class
    {
        var row = await Project(q).SingleOrDefaultAsync(ct); return row is null ? null : mapper.Map<TDto>(row);
    }
    public Task<PagedResult<HotelListItemDto>> GetApprovedListAsync(string? search, string? city, int page, int pageSize, CancellationToken cancellationToken = default) =>
        List<HotelListItemDto>(Filter(Base().Where(x => x.ModerationStatus == HotelModerationStatus.Approved), search, city), page, pageSize, cancellationToken);
    public Task<HotelDetailsDto?> GetApprovedByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        One<HotelDetailsDto>(Base().Where(x => x.Id == id && x.ModerationStatus == HotelModerationStatus.Approved), cancellationToken);
    public Task<PagedResult<HotelManagementListItemDto>> GetOwnerListAsync(Guid ownerId, string? search, string? city, HotelModerationStatus? status, int page, int pageSize, CancellationToken cancellationToken = default) =>
        List<HotelManagementListItemDto>(Filter(Base().Where(x => x.OwnerId == ownerId), search, city, status), page, pageSize, cancellationToken);
    public Task<HotelManagementDetailsDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken cancellationToken = default) =>
        One<HotelManagementDetailsDto>(Base().Where(x => x.OwnerId == ownerId && x.Id == id), cancellationToken);
    public Task<PagedResult<HotelManagementListItemDto>> GetModerationListAsync(string? search, string? city, HotelModerationStatus? status, int page, int pageSize, CancellationToken cancellationToken = default) =>
        List<HotelManagementListItemDto>(Filter(Base(), search, city, status), page, pageSize, cancellationToken);
    public Task<HotelManagementDetailsDto?> GetForModerationByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        One<HotelManagementDetailsDto>(Base().Where(x => x.Id == id), cancellationToken);
}
