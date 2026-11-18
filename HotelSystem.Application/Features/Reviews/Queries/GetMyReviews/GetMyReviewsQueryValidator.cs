using FluentValidation;
using HotelSystem.Application.Features.Reviews.Validation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviews;

public sealed class GetMyReviewsQueryValidator : AbstractValidator<GetMyReviewsQuery>
{
    public GetMyReviewsQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.Filter).SetValidator(new ReviewFilterValidator()!).When(x => x.Filter is not null);
    }
}
