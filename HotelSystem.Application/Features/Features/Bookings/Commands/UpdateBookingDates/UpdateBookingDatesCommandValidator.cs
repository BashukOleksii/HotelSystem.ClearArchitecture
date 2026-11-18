using FluentValidation;
using HotelSystem.Application.Common.Time;

namespace HotelSystem.Application.Features.Bookings.Commands.UpdateBookingDates;

public sealed class UpdateBookingDatesCommandValidator : AbstractValidator<UpdateBookingDatesCommand>
{
    public UpdateBookingDatesCommandValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("Вкажіть бронювання.");
        RuleFor(x => x).Must(x => x.CheckIn.HasValue || x.CheckOut.HasValue)
            .WithMessage("Вкажіть хоча б одну нову дату.").OverridePropertyName("Dates");
        RuleFor(x => x.CheckIn).Must(x => x is null || x.Value >= calendar.Today)
            .WithMessage("Дата заїзду не може бути в минулому.");
        RuleFor(x => x.CheckOut).Must(x => x is null || x.Value > calendar.Today)
            .WithMessage("Дата виїзду має бути пізнішою за сьогодні.");
        RuleFor(x => x.CheckOut).Must((x, to) => to is null || x.CheckIn is null || to > x.CheckIn)
            .WithMessage("Дата виїзду має бути пізнішою за дату заїзду.");
    }
}
