using HotelSystem.Application.Features.Users.DTOs;
namespace HotelSystem.API.Contracts.Users;
public sealed record UpdateAccountRequest(UserDetailsInput Details, string CurrentPassword);
