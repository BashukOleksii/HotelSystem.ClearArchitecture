using FluentValidation;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomPrice;
public sealed class ChangeRoomPriceCommandValidator : AbstractValidator<ChangeRoomPriceCommand>
{
    public ChangeRoomPriceCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.CostPerNight).Must(x => x > 0 && x <= RoomRules.MaximumPrice && decimal.Round(x, 2) == x)
            .WithMessage("Ціна має відповідати decimal(18,2) та бути додатною.");
    }
}
