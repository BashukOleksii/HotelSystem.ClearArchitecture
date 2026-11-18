using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Hotels.DTOs;

public sealed record HotelListItemDto(
    Guid Id,
    string Name,
    string City,
    IReadOnlyList<PhotoDto> Photos);