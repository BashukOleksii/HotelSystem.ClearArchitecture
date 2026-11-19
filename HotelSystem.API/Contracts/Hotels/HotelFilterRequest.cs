using HotelSystem.Domain.Enums;

namespace HotelSystem.API.Contracts.Hotels;

public sealed record HotelFilterRequest(string? Search = null, string? City = null,
    HotelModerationStatus? Status = null, int Page = 1, int PageSize = 10);
