using FluentValidation;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookingById;

public sealed class GetOwnerBookingByIdQueryValidator : AbstractValidator<GetOwnerBookingByIdQuery>
{
    public GetOwnerBookingByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Потрібно визначити виконавця операції.");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("Вкажіть бронювання.");
    }
}
