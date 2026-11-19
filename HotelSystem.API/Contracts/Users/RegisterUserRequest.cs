using HotelSystem.Application.Features.Users.DTOs;
namespace HotelSystem.API.Contracts.Users;
public sealed record RegisterUserRequest(UserDetailsInput Details, string Password);
