using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotelById;

public sealed class GetMyHotelByIdQueryValidator
    : AbstractValidator<GetMyHotelByIdQuery>
{
    public GetMyHotelByIdQueryValidator()
    {
        RuleFor(query => query.ActorId)
            .NotEmpty()
            .WithMessage(
                "Потрібно визначити виконавця операції.");

        RuleFor(query => query.HotelId)
            .NotEmpty()
            .WithMessage(
                "Вкажіть ідентифікатор готелю.");
    }
}