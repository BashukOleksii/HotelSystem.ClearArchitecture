using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.RevokeUserRole;

public sealed class RevokeUserRoleCommandValidator : AbstractValidator<RevokeUserRoleCommand>
{
    public RevokeUserRoleCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty().MaximumLength(50);
    }
}
