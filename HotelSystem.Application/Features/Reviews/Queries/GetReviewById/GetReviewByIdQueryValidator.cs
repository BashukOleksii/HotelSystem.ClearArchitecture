using FluentValidation;

namespace HotelSystem.Application.Features.Reviews.Queries.GetReviewById;

public sealed class GetReviewByIdQueryValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdQueryValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().WithMessage("Вкажіть відгук.");
    }
}
