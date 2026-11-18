using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Commands.DeleteReviewAsAdmin;

public sealed class DeleteReviewAsAdminCommandValidator : AbstractValidator<DeleteReviewAsAdminCommand>
{
    public DeleteReviewAsAdminCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
