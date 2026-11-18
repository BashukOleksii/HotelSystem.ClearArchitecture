using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Enums;

namespace HotelSystem.Application.Features.Hotels.Queries;

public interface IHotelReadService
{
    Task<PagedResult<HotelListItemDto>> GetApprovedListAsync(
        string? search,
        string? city,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<HotelDetailsDto?> GetApprovedByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<HotelManagementListItemDto>> GetOwnerListAsync(
        Guid ownerId,
        string? search,
        string? city,
        HotelModerationStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<HotelManagementDetailsDto?> GetOwnerByIdAsync(
        Guid ownerId,
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<HotelManagementListItemDto>> GetModerationListAsync(
        string? search,
        string? city,
        HotelModerationStatus? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<HotelManagementDetailsDto?> GetForModerationByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}