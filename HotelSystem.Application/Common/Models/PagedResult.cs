namespace HotelSystem.Application.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    long TotalCount)
{
    public long TotalPages =>
        TotalCount / PageSize
        + (TotalCount % PageSize == 0 ? 0 : 1);
}