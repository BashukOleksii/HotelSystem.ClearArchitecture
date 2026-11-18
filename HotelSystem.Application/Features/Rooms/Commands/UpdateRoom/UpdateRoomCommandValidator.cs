using FluentValidation;
using HotelSystem.Application.Features.Rooms.Validation;

namespace HotelSystem.Application.Features.Rooms.Commands.UpdateRoom;
public sealed class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Details).NotNull().WithMessage("Вкажіть значення.").SetValidator(new RoomDetailsInputValidator());
    }
}
