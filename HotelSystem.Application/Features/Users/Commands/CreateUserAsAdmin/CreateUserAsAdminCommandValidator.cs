using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.CreateUserAsAdmin;

public sealed class CreateUserAsAdminCommandValidator : AbstractValidator<CreateUserAsAdminCommand>
{
    public CreateUserAsAdminCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.Details).NotNull().SetValidator(new UserDetailsInputValidator());
        RuleFor(x => x.Password).Must(PasswordRules.IsValidNew)
            .WithMessage("Новий пароль: щонайменше 15 символів, до 72 байтів UTF-8, без нульового символу.");
    }
}
