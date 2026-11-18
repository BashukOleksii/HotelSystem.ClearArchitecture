using FluentValidation;
using HotelSystem.Application.Features.Users.Validation;

namespace HotelSystem.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
