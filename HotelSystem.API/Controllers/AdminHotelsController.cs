using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Hotels;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.Commands.ModerateHotel;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Application.Features.Hotels.Queries.GetHotelForModerationById;
using HotelSystem.Application.Features.Hotels.Queries.GetHotelsForModeration;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/admin/hotels")]
[Authorize(Roles = RoleNames.Admin)]
public sealed class AdminHotelsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<HotelManagementListItemDto>>> GetList([FromQuery] HotelFilterRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new GetHotelsForModerationQuery(User.RequiredUserId(), request.Search, request.City, request.Status, request.Page, request.PageSize), ct));

    [HttpGet("{hotelId:guid}")]
    public async Task<ActionResult<HotelManagementDetailsDto>> GetById(Guid hotelId, CancellationToken ct) =>
        Ok(await sender.Send(new GetHotelForModerationByIdQuery(User.RequiredUserId(), hotelId), ct));

    [HttpPut("{hotelId:guid}/moderation")]
    public async Task<IActionResult> Moderate(Guid hotelId, ModerateHotelRequest request, CancellationToken ct)
    {
        await sender.Send(new ModerateHotelCommand(User.RequiredUserId(), hotelId, request.Status!.Value), ct);
        return NoContent();
    }
}
