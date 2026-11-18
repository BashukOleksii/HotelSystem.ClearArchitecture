using FluentValidation;

namespace HotelSystem.Application.Features.Rooms.Commands.ChangeRoomCapacity;
public sealed class ChangeRoomCapacityCommandValidator : AbstractValidator<ChangeRoomCapacityCommand>
{
    public ChangeRoomCapacityCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Значення має бути додатним.");
    }
}
