namespace HotelSystem.Application.Features.Users.DTOs;

public sealed record UserDetailsInput(string Username, string Email, string? PhoneNumber = null);
