using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Commands.RemoveReviewPhoto;

public sealed class RemoveReviewPhotoCommandValidator : AbstractValidator<RemoveReviewPhotoCommand>
{
    public RemoveReviewPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
        RuleFor(x => x.PhotoId).NotEmpty().WithMessage("Вкажіть фотографію.");
    }
}
