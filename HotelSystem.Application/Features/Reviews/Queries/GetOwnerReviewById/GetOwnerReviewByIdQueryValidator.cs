using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviewById;

public sealed class GetOwnerReviewByIdQueryValidator : AbstractValidator<GetOwnerReviewByIdQuery>
{
    public GetOwnerReviewByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
