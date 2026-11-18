using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Queries;

namespace HotelSystem.Application.Features.Rooms.Validation;
public sealed class RoomStayValidator : AbstractValidator<RoomStay>
{
    public RoomStayValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.CheckIn).Must(x => x >= calendar.Today)
            .WithMessage("Дата заїзду не може бути в минулому.");
        RuleFor(x => x.CheckOut).GreaterThan(x => x.CheckIn)
            .WithMessage("Дата виїзду має бути пізнішою за дату заїзду.");
    }
}
