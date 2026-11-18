namespace HotelSystem.Application.Features.Users.DTOs;

public sealed record AccessTokenDto(string AccessToken, DateTimeOffset ExpiresAt, string TokenType = "Bearer");
