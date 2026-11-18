using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomForModerationById;
public sealed class GetRoomForModerationByIdQueryHandler(IUserRepository users, IRoomReadService reads) : IRequestHandler<GetRoomForModerationByIdQuery, RoomManagementDto>
{
    public async Task<RoomManagementDto> Handle(GetRoomForModerationByIdQuery request, CancellationToken cancellationToken)
    {
        await RoomAccess.RequireRoleAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        var result = await reads.GetForModerationByIdAsync(request.RoomId, cancellationToken);
        return result ?? throw new NotFoundException("Номер не знайдено.");
    }
}
