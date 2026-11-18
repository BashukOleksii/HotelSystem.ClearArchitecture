using FluentValidation;

namespace HotelSystem.Application.Features.Hotels.Commands.RemoveHotelPhoto;

public sealed class RemoveHotelPhotoCommandValidator : AbstractValidator<RemoveHotelPhotoCommand>
{
    public RemoveHotelPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.HotelId).NotEmpty();
        RuleFor(x => x.PhotoId).NotEmpty();
    }
}
