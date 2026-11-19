using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Common;
using HotelSystem.API.Contracts.Hotels;
using HotelSystem.API.Contracts.Photos;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.Commands.AddHotelPhoto;
using HotelSystem.Application.Features.Hotels.Commands.CreateHotel;
using HotelSystem.Application.Features.Hotels.Commands.DeleteHotel;
using HotelSystem.Application.Features.Hotels.Commands.RemoveHotelPhoto;
using HotelSystem.Application.Features.Hotels.Commands.UpdateHotel;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Application.Features.Hotels.Queries.GetMyHotelById;
using HotelSystem.Application.Features.Hotels.Queries.GetMyHotels;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/owner/hotels")]
[Authorize(Roles = RoleNames.Owner)]
public sealed class OwnerHotelsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<HotelManagementListItemDto>>> GetList([FromQuery] HotelFilterRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyHotelsQuery(User.RequiredUserId(), request.Search, request.City, request.Status, request.Page, request.PageSize), ct));

    [HttpGet("{hotelId:guid}")]
    public async Task<ActionResult<HotelManagementDetailsDto>> GetById(Guid hotelId, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyHotelByIdQuery(User.RequiredUserId(), hotelId), ct));

    [HttpPost]
    [ProducesResponseType<CreatedResourceResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedResourceResponse>> Create(HotelDetailsRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateHotelCommand(User.RequiredUserId(), request.Name, request.City, request.Street, request.Number, request.Description), ct);
        return CreatedAtAction(nameof(GetById), new { hotelId = id }, new CreatedResourceResponse(id));
    }

    [HttpPut("{hotelId:guid}")]
    public async Task<IActionResult> Update(Guid hotelId, HotelDetailsRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateHotelCommand(User.RequiredUserId(), hotelId, request.Name, request.City, request.Street, request.Number, request.Description), ct);
        return NoContent();
    }

    [HttpDelete("{hotelId:guid}")]
    public async Task<IActionResult> Delete(Guid hotelId, CancellationToken ct)
    {
        await sender.Send(new DeleteHotelCommand(User.RequiredUserId(), hotelId), ct);
        return NoContent();
    }

    [HttpPost("{hotelId:guid}/photos")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
    [ProducesResponseType<PhotoDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<PhotoDto>> AddPhoto(Guid hotelId, [FromForm] PhotoUploadRequest request, CancellationToken ct)
    {
        await using var content = request.File.OpenReadStream();
        var photo = await sender.Send(new AddHotelPhotoCommand(User.RequiredUserId(), hotelId,
            new PhotoUpload(content, request.File.FileName, request.File.ContentType)), ct);
        return Created(photo.Url, photo);
    }

    [HttpDelete("{hotelId:guid}/photos/{photoId:guid}")]
    public async Task<IActionResult> RemovePhoto(Guid hotelId, Guid photoId, CancellationToken ct)
    {
        await sender.Send(new RemoveHotelPhotoCommand(User.RequiredUserId(), hotelId, photoId), ct);
        return NoContent();
    }
}
