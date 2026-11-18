using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;

namespace HotelSystem.Application.Features.Rooms.Queries;

public interface IRoomReadService
{
    Task<PagedResult<RoomListItemDto>> SearchPublicAsync(RoomFilter filter, CancellationToken ct = default);
    Task<RoomDetailsDto?> GetPublicByIdAsync(Guid id, RoomStay? stay, CancellationToken ct = default);
    Task<PagedResult<RoomManagementDto>> GetOwnerListAsync(Guid ownerId, RoomFilter filter, CancellationToken ct = default);
    Task<RoomManagementDto?> GetOwnerByIdAsync(Guid ownerId, Guid id, CancellationToken ct = default);
    Task<PagedResult<RoomManagementDto>> GetModerationListAsync(RoomFilter filter, CancellationToken ct = default);
    Task<RoomManagementDto?> GetForModerationByIdAsync(Guid id, CancellationToken ct = default);
}
