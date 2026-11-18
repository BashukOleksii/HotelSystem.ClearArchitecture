using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public sealed class GetHotelReviewSummaryQueryValidator : AbstractValidator<GetHotelReviewSummaryQuery>
{
    public GetHotelReviewSummaryQueryValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Вкажіть готель.");
    }
}
