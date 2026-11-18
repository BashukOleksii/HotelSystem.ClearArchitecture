using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomById;
public sealed class GetRoomByIdQueryHandler(IRoomReadService reads) : IRequestHandler<GetRoomByIdQuery, RoomDetailsDto>
{
    public async Task<RoomDetailsDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await reads.GetPublicByIdAsync(request.RoomId, request.Stay, cancellationToken);
        return result ?? throw new NotFoundException("Номер не знайдено.");
    }
}
