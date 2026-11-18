using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.ChangeMyPassword;

public sealed class ChangeMyPasswordCommandValidator : AbstractValidator<ChangeMyPasswordCommand>
{
    public ChangeMyPasswordCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.CurrentPassword).Must(PasswordRules.CanVerify).WithMessage("Вкажіть поточний пароль до 72 байтів UTF-8.");
        RuleFor(x => x.NewPassword).Must(PasswordRules.IsValidNew)
            .WithMessage("Новий пароль: щонайменше 15 символів, до 72 байтів UTF-8, без нульового символу.");
    }
}
