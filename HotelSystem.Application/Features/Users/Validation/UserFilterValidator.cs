using FluentValidation;
using HotelSystem.Application.Features.Users.Queries;

namespace HotelSystem.Application.Features.Users.Validation;

public sealed class UserFilterValidator : AbstractValidator<UserFilter>
{
    public UserFilterValidator()
    {
        RuleFor(x => x.Search).MaximumLength(100);
        RuleFor(x => x.RoleName).MaximumLength(50);
        RuleFor(x => x.SortBy).IsInEnum();
        RuleFor(x => x.Page).GreaterThan(0)
            .Must((x, page) => x.PageSize <= 0 || ((long)page - 1) * x.PageSize <= int.MaxValue);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
