namespace HotelSystem.Application.Features.Users.DTOs;

public sealed record UserAccountDto(Guid Id, string Username, string Email, string? PhoneNumber,
    bool IsBlocked, DateTimeOffset CreatedAt, IReadOnlyList<string> Roles,
    OwnerProfileDto? OwnerProfile, AdminProfileDto? AdminProfile);

public sealed record OwnerProfileDto(string? CompanyName, string? BusinessPhone);
public sealed record AdminProfileDto(string? EmployeeNumber, string? DepartmentName);
