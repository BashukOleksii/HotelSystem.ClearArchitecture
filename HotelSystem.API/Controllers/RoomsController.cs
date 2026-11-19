using HotelSystem.API.Contracts.Rooms;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Application.Features.Rooms.Queries.GetRoomById;
using HotelSystem.Application.Features.Rooms.Queries.SearchRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/rooms")]
[AllowAnonymous]
public sealed class RoomsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<RoomListItemDto>>> Search([FromQuery] RoomSearchRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new SearchRoomsQuery(request.ToFilter()), ct));

    [HttpGet("{roomId:guid}")]
    public async Task<ActionResult<RoomDetailsDto>> GetById(Guid roomId, [FromQuery] RoomStayRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new GetRoomByIdQuery(roomId, request.ToStay()), ct));
}
