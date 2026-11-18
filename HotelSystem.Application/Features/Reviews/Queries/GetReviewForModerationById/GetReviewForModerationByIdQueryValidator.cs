using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewForModerationById;

public sealed class GetReviewForModerationByIdQueryValidator : AbstractValidator<GetReviewForModerationByIdQuery>
{
    public GetReviewForModerationByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
