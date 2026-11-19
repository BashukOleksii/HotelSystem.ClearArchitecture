using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Common;
using HotelSystem.API.Contracts.Photos;
using HotelSystem.API.Contracts.Rooms;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.Commands.AddRoomPhoto;
using HotelSystem.Application.Features.Rooms.Commands.ChangeRoomCapacity;
using HotelSystem.Application.Features.Rooms.Commands.ChangeRoomPrice;
using HotelSystem.Application.Features.Rooms.Commands.CreateRoom;
using HotelSystem.Application.Features.Rooms.Commands.DeleteRoom;
using HotelSystem.Application.Features.Rooms.Commands.RemoveRoomPhoto;
using HotelSystem.Application.Features.Rooms.Commands.SetRoomAvailability;
using HotelSystem.Application.Features.Rooms.Commands.UpdateRoom;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Application.Features.Rooms.Models;
using HotelSystem.Application.Features.Rooms.Queries.GetMyRoomById;
using HotelSystem.Application.Features.Rooms.Queries.GetMyRooms;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/owner/rooms")]
[Authorize(Roles = RoleNames.Owner)]
public sealed class OwnerRoomsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<RoomManagementDto>>> GetList([FromQuery] RoomSearchRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyRoomsQuery(User.RequiredUserId(), request.ToFilter()), ct));

    [HttpGet("{roomId:guid}")]
    public async Task<ActionResult<RoomManagementDto>> GetById(Guid roomId, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyRoomByIdQuery(User.RequiredUserId(), roomId), ct));

    [HttpPost("/api/owner/hotels/{hotelId:guid}/rooms")]
    [ProducesResponseType<CreatedResourceResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedResourceResponse>> Create(Guid hotelId, RoomDetailsInput request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateRoomCommand(User.RequiredUserId(), hotelId, request), ct);
        return CreatedAtAction(nameof(GetById), new { roomId = id }, new CreatedResourceResponse(id));
    }

    [HttpPut("{roomId:guid}")]
    public async Task<IActionResult> Update(Guid roomId, RoomDetailsInput request, CancellationToken ct)
    {
        await sender.Send(new UpdateRoomCommand(User.RequiredUserId(), roomId, request), ct);
        return NoContent();
    }

    [HttpPut("{roomId:guid}/price")]
    public async Task<IActionResult> ChangePrice(Guid roomId, ChangeRoomPriceRequest request, CancellationToken ct)
    {
        await sender.Send(new ChangeRoomPriceCommand(User.RequiredUserId(), roomId, request.CostPerNight), ct);
        return NoContent();
    }

    [HttpPut("{roomId:guid}/capacity")]
    public async Task<IActionResult> ChangeCapacity(Guid roomId, ChangeRoomCapacityRequest request, CancellationToken ct)
    {
        await sender.Send(new ChangeRoomCapacityCommand(User.RequiredUserId(), roomId, request.Capacity), ct);
        return NoContent();
    }

    [HttpPut("{roomId:guid}/availability")]
    public async Task<IActionResult> SetAvailability(Guid roomId, SetRoomAvailabilityRequest request, CancellationToken ct)
    {
        await sender.Send(new SetRoomAvailabilityCommand(User.RequiredUserId(), roomId, request.IsAvailable!.Value), ct);
        return NoContent();
    }

    [HttpDelete("{roomId:guid}")]
    public async Task<IActionResult> Delete(Guid roomId, CancellationToken ct)
    {
        await sender.Send(new DeleteRoomCommand(User.RequiredUserId(), roomId), ct);
        return NoContent();
    }

    [HttpPost("{roomId:guid}/photos")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
    [ProducesResponseType<PhotoDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<PhotoDto>> AddPhoto(Guid roomId, [FromForm] PhotoUploadRequest request, CancellationToken ct)
    {
        await using var content = request.File.OpenReadStream();
        var photo = await sender.Send(new AddRoomPhotoCommand(User.RequiredUserId(), roomId,
            new PhotoUpload(content, request.File.FileName, request.File.ContentType)), ct);
        return Created(photo.Url, photo);
    }

    [HttpDelete("{roomId:guid}/photos/{photoId:guid}")]
    public async Task<IActionResult> RemovePhoto(Guid roomId, Guid photoId, CancellationToken ct)
    {
        await sender.Send(new RemoveRoomPhotoCommand(User.RequiredUserId(), roomId, photoId), ct);
        return NoContent();
    }
}
