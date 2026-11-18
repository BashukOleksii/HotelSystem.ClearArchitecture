using FluentValidation;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Hotels.Commands.ModerateHotel;

public sealed class ModerateHotelCommandValidator
    : AbstractValidator<ModerateHotelCommand>
{
    public ModerateHotelCommandValidator()
    {
        RuleFor(command => command.ActorId)
            .NotEmpty()
            .WithMessage(
                "Потрібно визначити виконавця операції.");

        RuleFor(command => command.HotelId)
            .NotEmpty()
            .WithMessage(
                "Вкажіть ідентифікатор готелю.");

        RuleFor(command => command.Status)
            .Must(status => status is
                HotelModerationStatus.Approved
                or HotelModerationStatus.Rejected)
            .WithMessage(
                "Рішення модерації має бути Approved або Rejected.");
    }
}