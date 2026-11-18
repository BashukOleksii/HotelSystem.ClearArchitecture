using FluentValidation;
using HotelSystem.Application.Features.Rooms.Validation;

namespace HotelSystem.Application.Features.Rooms.Commands.CreateRoom;
public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Details).NotNull().WithMessage("Вкажіть значення.").SetValidator(new RoomDetailsInputValidator());
    }
}
