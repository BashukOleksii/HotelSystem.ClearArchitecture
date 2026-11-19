using HotelSystem.Domain.Enums;
using HotelSystem.Infrastructure.DTOs.Photos;

namespace HotelSystem.Infrastructure.DTOs.Reviews;

internal sealed record ReviewReadData(Guid Id, Guid HotelId, string HotelName, Guid UserId,
    string Username, int Rating, string? Comment, DateTimeOffset CreatedAt,
    bool IsDeleted, DateTimeOffset? DeletedAt, Guid? DeletedByUserId, Guid Version,
    bool AuthorIsBlocked, bool HotelIsDeleted, HotelModerationStatus HotelModerationStatus,
    List<PhotoReadData> Photos);
