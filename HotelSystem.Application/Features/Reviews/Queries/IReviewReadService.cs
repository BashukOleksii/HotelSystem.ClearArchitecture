using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;

namespace HotelSystem.Application.Features.Reviews.Queries;

public interface IReviewReadService
{
    Task<PagedResult<ReviewDto>> GetPublicHotelListAsync(Guid hotelId, ReviewFilter filter, CancellationToken ct = default);
    Task<ReviewDto?> GetPublicByIdAsync(Guid id, CancellationToken ct = default);
    Task<HotelReviewSummaryDto> GetPublicHotelSummaryAsync(Guid hotelId, CancellationToken ct = default);
    Task<PagedResult<ReviewDto>> GetClientListAsync(Guid userId, ReviewFilter filter, CancellationToken ct = default);
    Task<ReviewDto?> GetClientByIdAsync(Guid userId, Guid id, CancellationToken ct = default);
    Task<PagedResult<ReviewDto>> GetOwnerListAsync(Guid ownerId, ReviewFilter filter, CancellationToken ct = default);
    Task<ReviewDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken ct = default);
    Task<PagedResult<ReviewModerationDto>> GetModerationListAsync(ReviewFilter filter,
        Guid? userId, bool includeDeleted, CancellationToken ct = default);
    Task<ReviewModerationDto?> GetForModerationByIdAsync(Guid id, CancellationToken ct = default);
}
