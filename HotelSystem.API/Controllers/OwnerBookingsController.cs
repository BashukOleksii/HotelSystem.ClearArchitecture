using HotelSystem.API.Authentication;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Application.Features.Bookings.Queries;
using HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookingById;
using HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookings;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/owner/bookings")]
[Authorize(Roles = RoleNames.Owner)]
public sealed class OwnerBookingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<BookingManagementDto>>> GetList([FromQuery] BookingFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetOwnerBookingsQuery(User.RequiredUserId(), filter), ct));

    [HttpGet("{bookingId:guid}")]
    public async Task<ActionResult<BookingManagementDto>> GetById(Guid bookingId, CancellationToken ct) =>
        Ok(await sender.Send(new GetOwnerBookingByIdQuery(User.RequiredUserId(), bookingId), ct));
}
