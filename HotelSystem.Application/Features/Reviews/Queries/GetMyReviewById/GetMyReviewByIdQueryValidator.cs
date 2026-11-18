using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetMyReviewById;

public sealed class GetMyReviewByIdQueryValidator : AbstractValidator<GetMyReviewByIdQuery>
{
    public GetMyReviewByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
