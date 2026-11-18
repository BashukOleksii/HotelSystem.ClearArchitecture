using FluentValidation;
using HotelSystem.Application.Common.Validation;

namespace HotelSystem.Application.Features.Hotels.Commands.AddHotelPhoto;

public sealed class AddHotelPhotoCommandValidator : AbstractValidator<AddHotelPhotoCommand>
{
    public AddHotelPhotoCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.HotelId).NotEmpty();
        RuleFor(x => x.Upload).NotNull().SetValidator(new PhotoUploadValidator());
    }
}
