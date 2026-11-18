using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Queries.GetMyAccount;

public sealed class GetMyAccountQueryValidator : AbstractValidator<GetMyAccountQuery>
{
    public GetMyAccountQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
    }
}
