using AutoMapper;
using HotelSystem.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence.ReadServices;

internal static class ReadQuery
{
    public const string CaseInsensitiveCollation = "Latin1_General_100_CI_AS";
    public const string Escape = "~";
    public static string Pattern(string text) => "%" + text.Replace("~", "~~").Replace("%", "~%")
        .Replace("_", "~_").Replace("[", "~[").Replace("]", "~]") + "%";

    public static async Task<PagedResult<TDto>> PageAsync<TSource, TData, TDto>(
        IQueryable<TSource> ordered, int page, int pageSize,
        Func<IQueryable<TSource>, IQueryable<TData>> project, IMapper mapper, CancellationToken ct)
    {
        if (page < 1 || pageSize is < 1 or > 100 || ((long)page - 1) * pageSize > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(page), "Некоректна пагінація.");
        var total = await ordered.LongCountAsync(ct);
        var data = await project(ordered.Skip((page - 1) * pageSize).Take(pageSize)).ToListAsync(ct);
        return new(data.Select(x => mapper.Map<TDto>(x)).ToArray(), page, pageSize, total);
    }
}
