using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Application.Features.Hotels.Queries.GetHotelById;
using HotelSystem.Application.Features.Hotels.Queries.GetHotels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/hotels")]
[AllowAnonymous]
public sealed class HotelsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<HotelListItemDto>>> GetList([FromQuery] GetHotelsQuery query, CancellationToken ct) =>
        Ok(await sender.Send(query, ct));

    [HttpGet("{hotelId:guid}")]
    public async Task<ActionResult<HotelDetailsDto>> GetById(Guid hotelId, CancellationToken ct) =>
        Ok(await sender.Send(new GetHotelByIdQuery(hotelId), ct));
}
