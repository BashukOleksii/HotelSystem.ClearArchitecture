using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyOwnerProfile;

public sealed class UpdateMyOwnerProfileCommandValidator : AbstractValidator<UpdateMyOwnerProfileCommand>
{
    public UpdateMyOwnerProfileCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.CompanyName).Must(x => x is null || x.Trim().Length <= 150);
        RuleFor(x => x.BusinessPhone).Must(x => x is null || x.Trim().Length <= 30);
    }
}
