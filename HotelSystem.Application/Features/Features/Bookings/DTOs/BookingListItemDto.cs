using HotelSystem.Application.Common.Models;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Bookings.DTOs;

public sealed record BookingListItemDto(
    Guid Id, Guid RoomId, Guid HotelId, string HotelName, string RoomNumber,
    DateOnly CheckIn, DateOnly CheckOut, int Nights,
    decimal PricePerNight, decimal TotalPrice, BookingStatus Status,
    DateTimeOffset CreatedAt, IReadOnlyList<PhotoDto> RoomPhotos);
