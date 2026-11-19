namespace HotelSystem.API.Contracts.Reviews;

public sealed record ReviewDetailsRequest(int Rating, string? Comment = null);
