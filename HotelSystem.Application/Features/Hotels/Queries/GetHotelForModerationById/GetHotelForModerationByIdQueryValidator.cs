using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelForModerationById;

public sealed class GetHotelForModerationByIdQueryValidator
    : AbstractValidator<GetHotelForModerationByIdQuery>
{
    public GetHotelForModerationByIdQueryValidator()
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