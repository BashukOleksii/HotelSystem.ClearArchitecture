using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Bookings;
using HotelSystem.API.Contracts.Common;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.Commands.CancelBooking;
using HotelSystem.Application.Features.Bookings.Commands.CreateBooking;
using HotelSystem.Application.Features.Bookings.Commands.UpdateBookingDates;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Application.Features.Bookings.Queries;
using HotelSystem.Application.Features.Bookings.Queries.GetMyBookingById;
using HotelSystem.Application.Features.Bookings.Queries.GetMyBookings;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize(Roles = RoleNames.User)]
public sealed class BookingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<BookingListItemDto>>> GetList([FromQuery] BookingFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyBookingsQuery(User.RequiredUserId(), filter), ct));

    [HttpGet("{bookingId:guid}")]
    public async Task<ActionResult<BookingDetailsDto>> GetById(Guid bookingId, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyBookingByIdQuery(User.RequiredUserId(), bookingId), ct));

    [HttpPost]
    [ProducesResponseType<CreatedResourceResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedResourceResponse>> Create(CreateBookingRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateBookingCommand(User.RequiredUserId(), request.RoomId, request.CheckIn, request.CheckOut), ct);
        return CreatedAtAction(nameof(GetById), new { bookingId = id }, new CreatedResourceResponse(id));
    }

    [HttpPatch("{bookingId:guid}/dates")]
    public async Task<IActionResult> UpdateDates(Guid bookingId, UpdateBookingDatesRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateBookingDatesCommand(User.RequiredUserId(), bookingId, request.CheckIn, request.CheckOut), ct);
        return NoContent();
    }

    [HttpPut("{bookingId:guid}/cancellation")]
    public async Task<IActionResult> Cancel(Guid bookingId, CancellationToken ct)
    {
        await sender.Send(new CancelBookingCommand(User.RequiredUserId(), bookingId), ct);
        return NoContent();
    }
}
