using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Application.Features.Users.Validation;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure.Persistence.Seeding;

public sealed class BootstrapAdminOptionsValidator : IValidateOptions<BootstrapAdminOptions>
{
    public ValidateOptionsResult Validate(string? name, BootstrapAdminOptions options)
    {
        if (!options.Enabled) return ValidateOptionsResult.Success;

        var failures = new UserDetailsInputValidator()
            .Validate(new UserDetailsInput(options.Username, options.Email, options.PhoneNumber))
            .Errors.Select(x => "BootstrapAdmin: " + x.ErrorMessage).ToList();
        if (!PasswordRules.IsValidNew(options.Password))
            failures.Add("BootstrapAdmin:Password має відповідати правилам нового пароля Application.");
        try { AdminProfile.Create(Guid.NewGuid(), options.EmployeeNumber, options.DepartmentName); }
        catch (DomainValidationException error) { failures.Add("BootstrapAdmin: " + error.Message); }

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
