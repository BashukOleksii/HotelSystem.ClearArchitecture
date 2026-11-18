using FluentValidation;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Reviews.Validation;

public sealed class ReviewFilterValidator : AbstractValidator<ReviewFilter>
{
    public ReviewFilterValidator()
    {
        RuleFor(x => x.Search).MaximumLength(100).WithMessage("Пошук має містити до 100 символів.");
        RuleFor(x => x.HotelId).NotEqual(Guid.Empty).When(x => x.HotelId.HasValue)
            .WithMessage("Ідентифікатор готелю не може бути порожнім.");
        RuleFor(x => x.MinRating).InclusiveBetween(ReviewRules.MinimumRating, ReviewRules.MaximumRating)
            .When(x => x.MinRating.HasValue).WithMessage("Оцінка має бути від 1 до 5.");
        RuleFor(x => x.MaxRating).InclusiveBetween(ReviewRules.MinimumRating, ReviewRules.MaximumRating)
            .When(x => x.MaxRating.HasValue).WithMessage("Оцінка має бути від 1 до 5.");
        RuleFor(x => x.MaxRating).Must((x, value) => value is null || x.MinRating is null || value >= x.MinRating)
            .WithMessage("Максимальна оцінка має бути не меншою за мінімальну.");
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("Невідоме сортування.");
        RuleFor(x => x.Page).Cascade(CascadeMode.Stop).GreaterThan(0).WithMessage("Номер сторінки має бути додатним.")
            .Must((x, page) => x.PageSize <= 0 || ((long)page - 1) * x.PageSize <= int.MaxValue)
            .WithMessage("Номер сторінки завеликий.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100.");
    }
}
