using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.GrantUserRole;

public sealed class GrantUserRoleCommandValidator : AbstractValidator<GrantUserRoleCommand>
{
    public GrantUserRoleCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty().MaximumLength(50);
    }
}
