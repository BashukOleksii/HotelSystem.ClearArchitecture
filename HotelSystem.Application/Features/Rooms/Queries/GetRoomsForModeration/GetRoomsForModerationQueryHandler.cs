using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomsForModeration;
public sealed class GetRoomsForModerationQueryHandler(IUserRepository users, IRoomReadService reads) : IRequestHandler<GetRoomsForModerationQuery, PagedResult<RoomManagementDto>>
{
    public async Task<PagedResult<RoomManagementDto>> Handle(GetRoomsForModerationQuery request, CancellationToken cancellationToken)
    {
        await RoomAccess.RequireRoleAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        return await reads.GetModerationListAsync((request.Filter ?? new RoomFilter()).Normalize(), cancellationToken);
    }
}
