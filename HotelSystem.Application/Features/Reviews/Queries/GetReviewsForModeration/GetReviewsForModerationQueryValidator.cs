using FluentValidation;
using HotelSystem.Application.Features.Reviews.Validation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewsForModeration;

public sealed class GetReviewsForModerationQueryValidator : AbstractValidator<GetReviewsForModerationQuery>
{
    public GetReviewsForModerationQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.Filter).SetValidator(new ReviewFilterValidator()!).When(x => x.Filter is not null);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty).When(x => x.UserId.HasValue).WithMessage("Ідентифікатор користувача не може бути порожнім.");
    }
}
