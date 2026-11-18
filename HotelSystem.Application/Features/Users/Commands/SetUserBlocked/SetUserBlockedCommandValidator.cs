using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.SetUserBlocked;

public sealed class SetUserBlockedCommandValidator : AbstractValidator<SetUserBlockedCommand>
{
    public SetUserBlockedCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
