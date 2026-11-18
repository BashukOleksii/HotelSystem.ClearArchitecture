using FluentValidation;

namespace HotelSystem.Application.Features.Rooms.Commands.SetRoomAvailability;
public sealed class SetRoomAvailabilityCommandValidator : AbstractValidator<SetRoomAvailabilityCommand>
{
    public SetRoomAvailabilityCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
    }
}
