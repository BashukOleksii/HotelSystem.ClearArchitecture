using HotelSystem.Infrastructure.DTOs.Users;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Application.Features.Users.Queries;
using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

public sealed class UserReadService(HotelDbContext db, IMapper mapper) : IUserReadService
{
    public Task<PagedResult<UserListItemDto>> SearchAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        db.EnsureUsable(); var f = filter.Normalize(); var q = db.Users.AsNoTracking();
        if (f.IsBlocked is { } blocked) q = q.Where(x => x.IsBlocked == blocked);
        if (f.RoleName is { } role)
        {
            var key = IdentityNormalization.Key(role); q = q.Where(x => x.Roles.Any(r => EF.Property<string>(r, "NormalizedName") == key));
        }
        if (f.Search is { } text)
        {
            var pattern = ReadQuery.Pattern(text);
            q = q.Where(x => EF.Functions.Like(EF.Functions.Collate(x.Username, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape)
                || EF.Functions.Like(EF.Functions.Collate(x.Email, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape)
                || EF.Functions.Like(EF.Functions.Collate(x.PhoneNumber, ReadQuery.CaseInsensitiveCollation), pattern, ReadQuery.Escape));
        }
        IOrderedQueryable<User> ordered = f.SortBy switch
        {
            UserSortBy.Email => f.Descending ? q.OrderByDescending(x => x.Email) : q.OrderBy(x => x.Email),
            UserSortBy.CreatedAt => f.Descending ? q.OrderByDescending(x => x.CreatedAt) : q.OrderBy(x => x.CreatedAt),
            _ => f.Descending ? q.OrderByDescending(x => x.Username) : q.OrderBy(x => x.Username)
        };
        return ReadQuery.PageAsync<User, UserReadData, UserListItemDto>(ordered.ThenBy(x => x.Id), f.Page, f.PageSize,
            source => source.Select(x => new UserReadData(x.Id, x.Username, x.Email, x.PhoneNumber, x.IsBlocked, x.CreatedAt,
                x.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToList())), mapper, cancellationToken);
    }
}
