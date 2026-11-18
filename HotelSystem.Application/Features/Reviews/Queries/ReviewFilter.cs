namespace HotelSystem.Application.Features.Reviews.Queries;

public sealed record ReviewFilter(string? Search = null, Guid? HotelId = null,
    int? MinRating = null, int? MaxRating = null,
    ReviewSortBy SortBy = ReviewSortBy.CreatedAt, bool Descending = true,
    int Page = 1, int PageSize = 10)
{
    public ReviewFilter Normalize() => this with
    {
        Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim()
    };
}
