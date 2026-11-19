namespace HotelSystem.API.Contracts.Hotels;

public sealed record HotelDetailsRequest(string Name, string City, string Street, string Number, string? Description = null);
