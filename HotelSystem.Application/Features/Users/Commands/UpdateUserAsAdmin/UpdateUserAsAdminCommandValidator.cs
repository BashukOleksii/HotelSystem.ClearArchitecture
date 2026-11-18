using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.UpdateUserAsAdmin;

public sealed class UpdateUserAsAdminCommandValidator : AbstractValidator<UpdateUserAsAdminCommand>
{
    public UpdateUserAsAdminCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Details).NotNull().SetValidator(new UserDetailsInputValidator());
    }
}
