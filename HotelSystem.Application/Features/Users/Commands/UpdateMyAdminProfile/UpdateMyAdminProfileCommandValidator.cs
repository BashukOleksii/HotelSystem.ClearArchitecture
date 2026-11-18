using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAdminProfile;

public sealed class UpdateMyAdminProfileCommandValidator : AbstractValidator<UpdateMyAdminProfileCommand>
{
    public UpdateMyAdminProfileCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.EmployeeNumber).Must(x => x is null || x.Trim().Length <= 50);
        RuleFor(x => x.DepartmentName).Must(x => x is null || x.Trim().Length <= 100);
    }
}
