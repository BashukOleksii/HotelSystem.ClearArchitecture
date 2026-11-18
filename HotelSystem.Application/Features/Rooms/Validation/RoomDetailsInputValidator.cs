using FluentValidation;
using HotelSystem.Application.Features.Rooms.Models;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Rooms.Validation;
public sealed class RoomDetailsInputValidator : AbstractValidator<RoomDetailsInput>
{
    public RoomDetailsInputValidator()
    {
        RuleFor(x => x.RoomNumber).NotEmpty().WithMessage("Поле є обов’язковим.").MaximumLength(RoomRules.MaximumNumberLength).WithMessage("Номер кімнати має містити до 20 символів.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("Невідоме значення переліку.");
        RuleFor(x => x.CostPerNight).Must(x => x > 0 && x <= RoomRules.MaximumPrice
            && decimal.Round(x, 2) == x).WithMessage("Ціна має відповідати decimal(18,2) та бути додатною.");
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Значення має бути додатним.")
            .Must((x, capacity) => !Enum.IsDefined(x.Type) || capacity <= RoomRules.MaximumCapacity(x.Type))
            .WithMessage("Місткість перевищує межу для цього типу номера.");
    }
}
