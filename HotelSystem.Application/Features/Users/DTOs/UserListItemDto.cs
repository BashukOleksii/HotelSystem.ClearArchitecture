namespace HotelSystem.Application.Features.Users.DTOs;

public sealed record UserListItemDto(Guid Id, string Username, string Email, string? PhoneNumber,
    bool IsBlocked, DateTimeOffset CreatedAt, IReadOnlyList<string> Roles);
