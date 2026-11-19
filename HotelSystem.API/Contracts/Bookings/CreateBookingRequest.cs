namespace HotelSystem.API.Contracts.Bookings;

public sealed record CreateBookingRequest(Guid RoomId, DateOnly CheckIn, DateOnly CheckOut);
