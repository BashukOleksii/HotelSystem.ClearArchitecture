using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Reviews.DTOs;

public sealed record ReviewModerationDto(ReviewDto Review, bool IsDeleted,
    DateTimeOffset? DeletedAt, Guid? DeletedByUserId, Guid Version,
    bool AuthorIsBlocked, bool HotelIsDeleted, HotelModerationStatus HotelModerationStatus);
