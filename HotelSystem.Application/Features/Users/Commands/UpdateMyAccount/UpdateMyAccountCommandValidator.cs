using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAccount;

public sealed class UpdateMyAccountCommandValidator : AbstractValidator<UpdateMyAccountCommand>
{
    public UpdateMyAccountCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.Details).NotNull().SetValidator(new UserDetailsInputValidator());
        RuleFor(x => x.CurrentPassword).Must(PasswordRules.CanVerify).WithMessage("Вкажіть поточний пароль до 72 байтів UTF-8.");
    }
}
