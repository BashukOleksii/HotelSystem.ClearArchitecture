

namespace HotelSystem.Application.Features.Bookings.DTOs;

public sealed record BookingManagementDto(BookingDetailsDto Booking,
    Guid UserId, string Username, string Email, Guid OwnerId);
