using HotelSystem.Domain.Enums;
using HotelSystem.Infrastructure.DTOs.Photos;

namespace HotelSystem.Infrastructure.DTOs.Hotels;

internal sealed record HotelReadData(Guid Id, Guid OwnerId, string Name, string? Description,
    string City, string Street, string Number, HotelModerationStatus ModerationStatus,
    DateTimeOffset CreatedAt, List<PhotoReadData> Photos);
