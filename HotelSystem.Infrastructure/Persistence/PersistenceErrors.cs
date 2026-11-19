using HotelSystem.Application.Common.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence;

internal static class PersistenceErrors
{
    public static Exception? Translate(Exception error)
    {
        if (error is DbUpdateConcurrencyException)
            return new ConflictException("Дані вже змінилися. Оновіть їх перед повторною операцією.");
        var sql = error as SqlException ?? error.InnerException as SqlException;
        if (sql is null) return null;
        if (sql.Number is 1205 or 3960 or 41302 or 41305 or 41325)
            return new ConflictException("Паралельна операція змінила дані. Повторіть запит після оновлення.");
        if (sql.Number is not (2601 or 2627)) return null;
        if (sql.Message.Contains("UX_Users_Normalized", StringComparison.Ordinal))
            return new ConflictException("Не вдалося використати ці облікові дані. Оберіть інші.");
        if (sql.Message.Contains("UX_Rooms_HotelId_RoomNumber", StringComparison.Ordinal))
            return new ConflictException("Такий номер кімнати вже використовується в цьому готелі.");
        if (sql.Message.Contains("UX_Reviews_HotelId_UserId", StringComparison.Ordinal))
            return new ConflictException("Ви вже залишили активний відгук про цей готель.");
        return new ConflictException("Операція конфліктує з уже збереженими даними.");
    }
}
