using HotelSystem.Application.Features.Rooms.Queries;
using HotelSystem.Domain.Enums;

namespace HotelSystem.API.Contracts.Rooms;

public sealed class RoomSearchRequest : RoomStayRequest
{
    public string? Search { get; set; }
    public Guid? HotelId { get; set; }
    public string? City { get; set; }
    public RoomType? Type { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinCapacity { get; set; }
    public bool? IsAvailable { get; set; }
    public RoomSortBy SortBy { get; set; } = RoomSortBy.Number;
    public bool Descending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public RoomFilter ToFilter() => new(Search, HotelId, City, Type, MinPrice, MaxPrice, MinCapacity,
        IsAvailable, ToStay(), SortBy, Descending, Page, PageSize);
}
