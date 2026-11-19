namespace HotelSystem.API.Contracts.Bookings;

public sealed record UpdateBookingDatesRequest(DateOnly? CheckIn = null, DateOnly? CheckOut = null);
