using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Entities;

public sealed class OwnerProfile
{
    public Guid UserId { get; private set; }
    public string? CompanyName { get; private set; }
    public string? BusinessPhone { get; private set; }

    private OwnerProfile() { }

    public static OwnerProfile Create(
        Guid userId,
        string? companyName = null,
        string? businessPhone = null)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(userId),
                "Вкажіть ідентифікатор користувача.");
        }

        var profile = new OwnerProfile
        {
            UserId = userId
        };

        profile.UpdateDetails(companyName, businessPhone);

        return profile;
    }

    public void UpdateDetails(
        string? companyName,
        string? businessPhone)
    {
        var normalizedCompanyName = NormalizeOptionalText(
            companyName, 150, nameof(companyName));

        var normalizedBusinessPhone = NormalizeOptionalText(
            businessPhone, 30, nameof(businessPhone));

        CompanyName = normalizedCompanyName;
        BusinessPhone = normalizedBusinessPhone;
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