using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Hotels.DTOs;

public sealed record HotelDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    string City,
    string Street,
    string Number,
    IReadOnlyList<PhotoDto> Photos);