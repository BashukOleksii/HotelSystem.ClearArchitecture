using FluentValidation;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRoomById;
public sealed class GetMyRoomByIdQueryValidator : AbstractValidator<GetMyRoomByIdQuery>
{
    public GetMyRoomByIdQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty().WithMessage("Поле є обов’язковим.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Поле є обов’язковим.");
    }
}
