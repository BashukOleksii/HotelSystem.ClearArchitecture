using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Bookings.Queries;

public sealed record BookingFilter(
    string? Search = null, Guid? HotelId = null, Guid? RoomId = null,
    BookingStatus? Status = null, DateOnly? From = null, DateOnly? To = null,
    BookingSortBy SortBy = BookingSortBy.CreatedAt, bool Descending = true,
    int Page = 1, int PageSize = 10)
{
    public BookingFilter Normalize() => this with
    {
        Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim()
    };
}
