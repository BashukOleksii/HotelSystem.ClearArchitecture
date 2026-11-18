using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Hotels.DTOs;

public sealed record HotelManagementListItemDto(
    Guid Id,
    Guid OwnerId,
    string Name,
    string City,
    HotelModerationStatus ModerationStatus,
    DateTimeOffset CreatedAt,
    IReadOnlyList<PhotoDto> Photos);