using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Reviews.DTOs;

public sealed record ReviewDto(Guid Id, Guid HotelId, string HotelName,
    Guid UserId, string Username, int Rating, string? Comment,
    DateTimeOffset CreatedAt, IReadOnlyList<PhotoDto> Photos);
