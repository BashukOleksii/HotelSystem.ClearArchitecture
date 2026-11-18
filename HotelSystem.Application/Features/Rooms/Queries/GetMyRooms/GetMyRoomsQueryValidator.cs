using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Validation;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRooms;
public sealed class GetMyRoomsQueryValidator : AbstractValidator<GetMyRoomsQuery>
{
    public GetMyRoomsQueryValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.Filter).SetValidator(new RoomFilterValidator(calendar)!).When(x => x.Filter is not null);
    }
}
