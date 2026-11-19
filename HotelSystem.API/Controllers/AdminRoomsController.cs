using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Rooms;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Application.Features.Rooms.Queries.GetRoomForModerationById;
using HotelSystem.Application.Features.Rooms.Queries.GetRoomsForModeration;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/admin/rooms")]
[Authorize(Roles = RoleNames.Admin)]
public sealed class AdminRoomsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<RoomManagementDto>>> GetList([FromQuery] RoomSearchRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new GetRoomsForModerationQuery(User.RequiredUserId(), request.ToFilter()), ct));

    [HttpGet("{roomId:guid}")]
    public async Task<ActionResult<RoomManagementDto>> GetById(Guid roomId, CancellationToken ct) =>
        Ok(await sender.Send(new GetRoomForModerationByIdQuery(User.RequiredUserId(), roomId), ct));
}
