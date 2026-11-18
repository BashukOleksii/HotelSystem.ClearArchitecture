using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Validation;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomById;
public sealed class GetRoomByIdQueryValidator : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdQueryValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Stay).SetValidator(new RoomStayValidator(calendar)!).When(x => x.Stay is not null);
    }
}
