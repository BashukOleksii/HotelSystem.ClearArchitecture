using FluentValidation;

namespace HotelSystem.Application.Features.Bookings.Commands.CancelBooking;

public sealed class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("Вкажіть бронювання.");
    }
}
