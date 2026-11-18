using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;

namespace HotelSystem.Application.Features.Hotels.Common;

internal static class HotelAccess
{
    public static async Task<Hotel> OwnedAsync(Guid actorId, Guid hotelId,
        IUserRepository users, IHotelRepository hotels, CancellationToken ct)
    {
        var actor = await users.GetByIdAsync(actorId, ct);
        if (actor is null || actor.IsBlocked || !actor.HasRole(RoleNames.Owner))
            throw new ForbiddenOperationException("Зміна галереї доступна активному користувачу з роллю Owner.");

        var hotel = await hotels.GetByIdAsync(hotelId, ct);
        if (hotel is null || hotel.IsDeleted)
            throw new NotFoundException("Готель не знайдено.");
        if (hotel.OwnerId != actor.Id)
            throw new ForbiddenOperationException("Ви можете змінювати лише галерею власного готелю.");

        return hotel;
    }
}
