using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelCommandValidator
    : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(command => command.ActorId)
            .NotEmpty()
            .WithMessage(
                "Потрібно визначити виконавця операції.");

        RuleFor(command => command.HotelId)
            .NotEmpty()
            .WithMessage(
                "Вкажіть ідентифікатор готелю.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage(
                "Вкажіть назву готелю.")
            .MaximumLength(100)
            .WithMessage(
                "Назва має містити до 100 символів.");

        RuleFor(command => command.City)
            .NotEmpty()
            .WithMessage(
                "Вкажіть місто.")
            .MaximumLength(100)
            .WithMessage(
                "Назва міста має містити до 100 символів.");

        RuleFor(command => command.Street)
            .NotEmpty()
            .WithMessage(
                "Вкажіть вулицю.")
            .MaximumLength(100)
            .WithMessage(
                "Назва вулиці має містити до 100 символів.");

        RuleFor(command => command.Number)
            .NotEmpty()
            .WithMessage(
                "Вкажіть номер будинку.")
            .MaximumLength(20)
            .WithMessage(
                "Номер будинку має містити до 20 символів.");

        RuleFor(command => command.Description)
            .MaximumLength(2000)
            .WithMessage(
                "Опис має містити до 2000 символів.");
    }
}