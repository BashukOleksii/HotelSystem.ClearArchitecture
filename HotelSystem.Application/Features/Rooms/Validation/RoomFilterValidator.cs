using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Features.Rooms.Queries;
using HotelSystem.Domain.Rules;

namespace HotelSystem.Application.Features.Rooms.Validation;
public sealed class RoomFilterValidator : AbstractValidator<RoomFilter>
{
    public RoomFilterValidator(BookingCalendar calendar)
    {
        RuleFor(x => x.Search).MaximumLength(100).WithMessage("Поле має містити до 100 символів.");
        RuleFor(x => x.City).MaximumLength(100).WithMessage("Поле має містити до 100 символів.");
        RuleFor(x => x.HotelId).NotEqual(Guid.Empty).WithMessage("Ідентифікатор не може бути порожнім.").When(x => x.HotelId.HasValue);
        RuleFor(x => x.Type).IsInEnum().WithMessage("Невідоме значення переліку.").When(x => x.Type.HasValue);
        RuleFor(x => x.SortBy).IsInEnum().WithMessage("Невідоме значення переліку.");
        RuleFor(x => x.MinPrice).Must(ValidPrice).WithMessage("Некоректна мінімальна ціна.");
        RuleFor(x => x.MaxPrice).Must(ValidPrice).WithMessage("Некоректна максимальна ціна.");
        RuleFor(x => x.MaxPrice).Must((x, value) => value is null || x.MinPrice is null || value >= x.MinPrice)
            .WithMessage("Максимальна ціна має бути не меншою за мінімальну.");
        RuleFor(x => x.MinCapacity).GreaterThan(0).WithMessage("Значення має бути додатним.").When(x => x.MinCapacity.HasValue);
        RuleFor(x => x.Stay).SetValidator(new RoomStayValidator(calendar)!).When(x => x.Stay is not null);
        RuleFor(x => x.IsAvailable).Must((x, value) => x.Stay is null || value != false)
            .WithMessage("Пошук на дати можливий лише для відкритих номерів.");
        RuleFor(x => x.Page).Cascade(CascadeMode.Stop).GreaterThan(0).WithMessage("Значення має бути додатним.")
            .Must((x, page) => x.PageSize <= 0 || ((long)page - 1) * x.PageSize <= int.MaxValue).WithMessage("Номер сторінки завеликий.");
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100.");
    }
    private static bool ValidPrice(decimal? x) => x is null
        || (x >= 0 && x <= RoomRules.MaximumPrice && decimal.Round(x.Value, 2) == x);
}
