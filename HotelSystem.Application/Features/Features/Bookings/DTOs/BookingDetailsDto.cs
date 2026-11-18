using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Bookings.DTOs;

public sealed record BookingDetailsDto(
    Guid Id, Guid RoomId, Guid HotelId, string HotelName, string RoomNumber,
    string City, string Street, string Number,
    DateOnly CheckIn, DateOnly CheckOut, int Nights,
    decimal PricePerNight, decimal TotalPrice, BookingStatus Status,
    DateTimeOffset CreatedAt, bool HotelIsDeleted, bool RoomIsDeleted,
    bool RoomIsAvailable, HotelModerationStatus HotelModerationStatus,
    IReadOnlyList<PhotoDto> RoomPhotos);
