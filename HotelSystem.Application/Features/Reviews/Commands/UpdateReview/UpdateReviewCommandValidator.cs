using FluentValidation;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
        RuleFor(x => x.Rating).InclusiveBetween(ReviewRules.MinimumRating, ReviewRules.MaximumRating).WithMessage("Оцінка має бути від 1 до 5.");
        RuleFor(x => x.Comment).MaximumLength(ReviewRules.MaximumCommentLength).WithMessage("Коментар має містити до 2000 символів.");
    }
}
