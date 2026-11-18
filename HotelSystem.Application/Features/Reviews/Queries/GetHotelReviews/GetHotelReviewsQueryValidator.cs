using FluentValidation;
using HotelSystem.Application.Features.Reviews.Validation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviews;

public sealed class GetHotelReviewsQueryValidator : AbstractValidator<GetHotelReviewsQuery>
{
    public GetHotelReviewsQueryValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Вкажіть готель.");
        RuleFor(x => x.Filter).SetValidator(new ReviewFilterValidator()!).When(x => x.Filter is not null);
        RuleFor(x => x).Must(x => x.Filter?.HotelId is null || x.Filter.HotelId == x.HotelId).WithMessage("Фільтр має відповідати обраному готелю.").OverridePropertyName("Filter.HotelId");
    }
}
