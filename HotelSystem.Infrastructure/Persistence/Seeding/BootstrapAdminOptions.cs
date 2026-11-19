namespace HotelSystem.Infrastructure.Persistence.Seeding;

public sealed class BootstrapAdminOptions
{
    public const string SectionName = "BootstrapAdmin";

    public bool Enabled { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? DepartmentName { get; set; }
}
