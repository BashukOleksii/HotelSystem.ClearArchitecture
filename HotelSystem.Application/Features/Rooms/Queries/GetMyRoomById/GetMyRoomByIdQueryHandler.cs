using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Rooms.Common;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRoomById;
public sealed class GetMyRoomByIdQueryHandler(IUserRepository users, IRoomReadService reads) : IRequestHandler<GetMyRoomByIdQuery, RoomManagementDto>
{
    public async Task<RoomManagementDto> Handle(GetMyRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await RoomAccess.RequireRoleAsync(request.ActorId, RoleNames.Owner, users, cancellationToken);
        var result = await reads.GetOwnerByIdAsync(actor.Id, request.RoomId, cancellationToken);
        return result ?? throw new NotFoundException("Номер не знайдено.");
    }
}
