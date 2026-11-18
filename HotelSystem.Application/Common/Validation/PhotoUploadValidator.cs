using FluentValidation;
using HotelSystem.Application.Common.Models;
namespace HotelSystem.Application.Common.Validation;
public sealed class PhotoUploadValidator : AbstractValidator<PhotoUpload>
{
    public PhotoUploadValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().WithMessage("Поле є обов’язковим.").MaximumLength(255).WithMessage("Назва файлу має містити до 255 символів.");
        RuleFor(x => x.ContentType).Must(x => x?.Trim().ToLowerInvariant()
            is "image/jpeg" or "image/png" or "image/webp")
            .WithMessage("Допустимі JPEG, PNG або WebP.");
        RuleFor(x => x.Content).Must(x => x is { CanRead: true, CanSeek: true }
            && x.Position == 0 && x.Length > 0 && x.Length <= PhotoUpload.MaximumBytes)
            .WithMessage("Потрібний читабельний потік на початку файлу розміром від 1 байта до 5 MiB.");
    }
}
