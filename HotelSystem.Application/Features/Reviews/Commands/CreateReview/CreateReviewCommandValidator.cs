using FluentValidation;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Вкажіть готель.");
        RuleFor(x => x.Rating).InclusiveBetween(ReviewRules.MinimumRating, ReviewRules.MaximumRating).WithMessage("Оцінка має бути від 1 до 5.");
        RuleFor(x => x.Comment).MaximumLength(ReviewRules.MaximumCommentLength).WithMessage("Коментар має містити до 2000 символів.");
    }
}
