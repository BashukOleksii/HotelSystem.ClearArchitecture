namespace HotelSystem.Application.Features.Users.Queries;

public enum UserSortBy { Username, Email, CreatedAt }

public sealed record UserFilter(string? Search = null, string? RoleName = null, bool? IsBlocked = null,
    UserSortBy SortBy = UserSortBy.Username, bool Descending = false, int Page = 1, int PageSize = 10)
{
    public UserFilter Normalize() => this with
    {
        Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim(),
        RoleName = string.IsNullOrWhiteSpace(RoleName) ? null : RoleName.Trim()
    };
}
