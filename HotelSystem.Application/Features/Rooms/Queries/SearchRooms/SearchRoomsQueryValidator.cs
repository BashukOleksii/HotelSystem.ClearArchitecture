using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Validation;

namespace HotelSystem.Application.Features.Rooms.Queries.SearchRooms;
public sealed class SearchRoomsQueryValidator : AbstractValidator<SearchRoomsQuery>
{
    public SearchRoomsQueryValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.Filter).SetValidator(new RoomFilterValidator(calendar)!).When(x => x.Filter is not null);
    }
}
