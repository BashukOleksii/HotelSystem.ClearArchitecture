using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
