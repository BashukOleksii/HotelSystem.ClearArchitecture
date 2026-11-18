using FluentValidation;

namespace HotelSystem.Application.Features.Bookings.Queries.GetMyBookingById;

public sealed class GetMyBookingByIdQueryValidator : AbstractValidator<GetMyBookingByIdQuery>
{
    public GetMyBookingByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("Вкажіть бронювання.");
    }
}
