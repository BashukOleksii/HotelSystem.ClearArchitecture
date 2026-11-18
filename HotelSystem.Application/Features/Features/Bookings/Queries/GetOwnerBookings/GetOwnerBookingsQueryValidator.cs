using FluentValidation;
using HotelSystem.Application.Features.Bookings.Validation;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookings;

public sealed class GetOwnerBookingsQueryValidator : AbstractValidator<GetOwnerBookingsQuery>
{
    public GetOwnerBookingsQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.Filter).SetValidator(new BookingFilterValidator()!).When(x => x.Filter is not null);
    }
}
