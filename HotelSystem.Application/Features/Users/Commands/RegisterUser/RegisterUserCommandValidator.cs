using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Details).NotNull().SetValidator(new UserDetailsInputValidator());
        RuleFor(x => x.Password).Must(PasswordRules.IsValidNew)
            .WithMessage("Новий пароль: щонайменше 15 символів, до 72 байтів UTF-8, без нульового символу.");
    }
}
