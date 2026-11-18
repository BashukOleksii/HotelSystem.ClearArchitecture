using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Interfaces;

namespace HotelSystem.Application.Features.Reviews.Common;

internal static class ReviewAccess
{
    public static async Task<User> RequireRoleAsync(Guid actorId, string role,
        IUserRepository users, CancellationToken ct)
    {
        var actor = await users.GetByIdAsync(actorId, ct);
        if (actor is null || actor.IsBlocked || !actor.HasRole(role))
            throw new ForbiddenOperationException($"Операція доступна активному користувачу з роллю {role}.");
        return actor;
    }

    public static async Task<Hotel> PublicHotelAsync(Guid hotelId, IHotelRepository hotels, CancellationToken ct)
    {
        var hotel = await hotels.GetByIdAsync(hotelId, ct);
        if (hotel is null || hotel.IsDeleted || hotel.ModerationStatus != HotelModerationStatus.Approved)
            throw new NotFoundException("Готель не знайдено.");
        return hotel;
    }

    public static async Task<Review> OwnedAsync(Guid actorId, Guid reviewId,
        IUserRepository users, IReviewRepository reviews, bool includeDeleted = false, CancellationToken ct = default)
    {
        var actor = await RequireRoleAsync(actorId, RoleNames.User, users, ct);
        var review = await reviews.GetByIdAsync(reviewId, ct);
        if (review is null) throw new NotFoundException("Відгук не знайдено.");
        if (review.UserId != actor.Id)
            throw new ForbiddenOperationException("Ви можете змінювати лише власні відгуки.");
        if (review.IsDeleted && !includeDeleted) throw new NotFoundException("Відгук не знайдено.");
        return review;
    }
}
