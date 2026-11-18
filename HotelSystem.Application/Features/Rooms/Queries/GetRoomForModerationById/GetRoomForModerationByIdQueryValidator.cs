using FluentValidation;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomForModerationById;
public sealed class GetRoomForModerationByIdQueryValidator : AbstractValidator<GetRoomForModerationByIdQuery>
{
    public GetRoomForModerationByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
    }
}
