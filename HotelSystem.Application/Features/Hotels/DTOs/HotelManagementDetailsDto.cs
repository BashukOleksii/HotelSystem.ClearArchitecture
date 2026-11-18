using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Hotels.DTOs;

public sealed record HotelManagementDetailsDto(
    Guid Id,
    Guid OwnerId,
    string Name,
    string? Description,
    string City,
    string Street,
    string Number,
    HotelModerationStatus ModerationStatus,
    DateTimeOffset CreatedAt,
    IReadOnlyList<PhotoDto> Photos);