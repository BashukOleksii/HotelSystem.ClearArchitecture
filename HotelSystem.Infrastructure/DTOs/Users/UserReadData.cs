namespace HotelSystem.Infrastructure.DTOs.Users;

internal sealed record UserReadData(Guid Id, string Username, string Email, string? PhoneNumber,
    bool IsBlocked, DateTimeOffset CreatedAt, List<string> Roles);
