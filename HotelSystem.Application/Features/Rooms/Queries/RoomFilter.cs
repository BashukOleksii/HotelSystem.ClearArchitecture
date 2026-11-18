using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Rooms.Queries;
public sealed record RoomFilter(
    string? Search = null, Guid? HotelId = null, string? City = null,
    RoomType? Type = null, decimal? MinPrice = null, decimal? MaxPrice = null,
    int? MinCapacity = null, bool? IsAvailable = null, RoomStay? Stay = null,
    RoomSortBy SortBy = RoomSortBy.Number, bool Descending = false,
    int Page = 1, int PageSize = 10)
{
    public RoomFilter Normalize() => this with
    {
        Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim(),
        City = string.IsNullOrWhiteSpace(City) ? null : City.Trim()
    };
}
