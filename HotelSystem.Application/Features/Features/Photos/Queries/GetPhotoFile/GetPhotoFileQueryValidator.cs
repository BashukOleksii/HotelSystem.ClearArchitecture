using FluentValidation;
namespace HotelSystem.Application.Features.Photos.Queries.GetPhotoFile;
public sealed class GetPhotoFileQueryValidator : AbstractValidator<GetPhotoFileQuery>
{
    public GetPhotoFileQueryValidator() => RuleFor(x => x.StorageKey).NotEmpty().MaximumLength(512);
}
