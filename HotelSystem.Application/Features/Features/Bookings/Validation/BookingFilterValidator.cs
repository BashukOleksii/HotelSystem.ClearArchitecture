using FluentValidation;
using HotelSystem.Application.Features.Bookings.Queries;

namespace HotelSystem.Application.Features.Bookings.Validation;

public sealed class BookingFilterValidator : AbstractValidator<BookingFilter>
{
    public BookingFilterValidator()
    {
        RuleFor(x => x.Search).MaximumLength(100).WithMessage("Пошук має містити до 100 символів.");
        RuleFor(x => x.HotelId).NotEqual(Guid.Empty).When(x => x.HotelId.HasValue)
            .WithMessage("Ідентифікатор готелю не може бути порожнім.");
        RuleFor(x => x.RoomId).NotEqual(Guid.Empty).When(x => x.RoomId.HasValue)
            .WithMessage("Ідентифікатор номера не може бути порожнім.");
        RuleFor(x => x.Status).IsInEnum().When(x => x.Status.HasValue)
            .WithMessage("Невідомий статус бронювання.");
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("Невідомий спосіб сортування.");
        RuleFor(x => x.To).Must((x, to) => to is null || x.From is null || to >= x.From)
            .WithMessage("Кінець діапазону має бути не раніше початку.");
        RuleFor(x => x.Page).Cascade(CascadeMode.Stop).GreaterThan(0)
            .WithMessage("Номер сторінки має бути додатним.")
            .Must((x, page) => x.PageSize <= 0 || ((long)page - 1) * x.PageSize <= int.MaxValue)
            .WithMessage("Номер сторінки завеликий.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100)
            .WithMessage("Розмір сторінки має бути від 1 до 100.");
    }
}
