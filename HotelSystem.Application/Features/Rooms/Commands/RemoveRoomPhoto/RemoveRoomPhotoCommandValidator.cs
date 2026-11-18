using FluentValidation;

namespace HotelSystem.Application.Features.Rooms.Commands.RemoveRoomPhoto;
public sealed class RemoveRoomPhotoCommandValidator : AbstractValidator<RemoveRoomPhotoCommand>
{
    public RemoveRoomPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.PhotoId).NotEmpty().WithMessage("Поле є обов’язковим.");
    }
}
