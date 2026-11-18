using FluentValidation;
using HotelSystem.Application.Common.Validation;

namespace HotelSystem.Application.Features.Rooms.Commands.AddRoomPhoto;
public sealed class AddRoomPhotoCommandValidator : AbstractValidator<AddRoomPhotoCommand>
{
    public AddRoomPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Upload).NotNull().WithMessage("Вкажіть значення.").SetValidator(new PhotoUploadValidator());
    }
}
