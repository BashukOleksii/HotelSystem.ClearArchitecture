namespace HotelSystem.API.Contracts.Users;
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
