using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Entities;

public sealed class AdminProfile
{
    public Guid UserId { get; private set; }
    public string? EmployeeNumber { get; private set; }
    public string? DepartmentName { get; private set; }

    private AdminProfile() { }

    public static AdminProfile Create(
        Guid userId,
        string? employeeNumber = null,
        string? departmentName = null)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(userId),
                "Вкажіть ідентифікатор користувача.");
        }

        var profile = new AdminProfile
        {
            UserId = userId
        };

        profile.UpdateDetails(employeeNumber, departmentName);

        return profile;
    }

    public void UpdateDetails(
        string? employeeNumber,
        string? departmentName)
    {
        var normalizedEmployeeNumber = NormalizeOptionalText(
            employeeNumber, 50, nameof(employeeNumber));

        var normalizedDepartmentName = NormalizeOptionalText(
            departmentName, 100, nameof(departmentName));

        EmployeeNumber = normalizedEmployeeNumber;
        DepartmentName = normalizedDepartmentName;
    }

    private static string? NormalizeOptionalText(
        string? value,
        int maximumLength,
        string field)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrEmpty(normalized))
            return null;

        if (normalized.Length > maximumLength)
        {
            throw new DomainValidationException(
                field,
                $"Поле може містити до {maximumLength} символів.");
        }

        return normalized;
    }
}