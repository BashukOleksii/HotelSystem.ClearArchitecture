namespace HotelSystem.Application.Common.Time;

public sealed class BookingCalendar(TimeProvider clock)
{
    public DateOnly Today => DateOnly.FromDateTime(clock.GetUtcNow().UtcDateTime);
}
