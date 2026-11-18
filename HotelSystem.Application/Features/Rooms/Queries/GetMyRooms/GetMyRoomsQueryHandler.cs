using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRooms;
public sealed class GetMyRoomsQueryHandler(IUserRepository users, IRoomReadService reads) : IRequestHandler<GetMyRoomsQuery, PagedResult<RoomManagementDto>>
{
    public async Task<PagedResult<RoomManagementDto>> Handle(GetMyRoomsQuery request, CancellationToken cancellationToken)
    {
        var actor = await RoomAccess.RequireRoleAsync(request.ActorId, RoleNames.Owner, users, cancellationToken);
        return await reads.GetOwnerListAsync(actor.Id, (request.Filter ?? new RoomFilter()).Normalize(), cancellationToken);
    }
}
