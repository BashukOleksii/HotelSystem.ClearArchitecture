using FluentValidation;
using HotelSystem.Application.Common.Validation;

namespace HotelSystem.Application.Features.Reviews.Commands.AddReviewPhoto;

public sealed class AddReviewPhotoCommandValidator : AbstractValidator<AddReviewPhotoCommand>
{
    public AddReviewPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
        RuleFor(x => x.Upload).NotNull().WithMessage("Додайте фотографію.").SetValidator(new PhotoUploadValidator());
    }
}
