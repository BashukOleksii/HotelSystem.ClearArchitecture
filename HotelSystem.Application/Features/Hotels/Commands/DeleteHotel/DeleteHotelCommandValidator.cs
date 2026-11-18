using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed class DeleteHotelCommandValidator
    : AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelCommandValidator()
    {
        RuleFor(command => command.ActorId)
            .NotEmpty()
            .WithMessage(
                "Потрібно визначити виконавця операції.");

        RuleFor(command => command.HotelId)
            .NotEmpty()
            .WithMessage(
                "Вкажіть ідентифікатор готелю.");
    }
}