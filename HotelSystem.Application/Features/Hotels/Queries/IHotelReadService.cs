using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;

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
}