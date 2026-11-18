using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Rules;

public static class BookingRules
{
    public const decimal MaximumTotalPrice = 9_999_999_999_999_999.99m;

    public static void EnsureDates(DateOnly checkIn, DateOnly checkOut, DateOnly currentDate)
    {
        if (checkIn < currentDate)
            throw new DomainValidationException(nameof(checkIn), "Дата заїзду не може бути в минулому.");
        if (checkOut <= checkIn)
            throw new DomainValidationException(nameof(checkOut), "Дата виїзду має бути пізнішою за дату заїзду.");
    }
}
