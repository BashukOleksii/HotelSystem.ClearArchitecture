using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed class GetHotelByIdQueryValidator
    : AbstractValidator<GetHotelByIdQuery>
{
    public GetHotelByIdQueryValidator()
    {
        RuleFor(query => query.HotelId)
            .NotEmpty()
            .WithMessage(
                "Вкажіть ідентифікатор готелю.");
    }
}