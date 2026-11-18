using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.SearchRooms;
public sealed class SearchRoomsQueryHandler(IRoomReadService reads) : IRequestHandler<SearchRoomsQuery, PagedResult<RoomListItemDto>>
{
    public async Task<PagedResult<RoomListItemDto>> Handle(SearchRoomsQuery request, CancellationToken cancellationToken)
    {
        return await reads.SearchPublicAsync((request.Filter ?? new RoomFilter()).Normalize(), cancellationToken);
    }
}
