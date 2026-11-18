using FluentValidation;
using HotelSystem.Application.Common.Time;

namespace HotelSystem.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Вкажіть номер готелю.");
        RuleFor(x => x.CheckIn).Must(x => x >= calendar.Today).WithMessage("Дата заїзду не може бути в минулому.");
        RuleFor(x => x.CheckOut).GreaterThan(x => x.CheckIn).WithMessage("Дата виїзду має бути пізнішою за дату заїзду.");
    }
}
