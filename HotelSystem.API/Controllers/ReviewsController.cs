using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Common;
using HotelSystem.API.Contracts.Photos;
using HotelSystem.API.Contracts.Reviews;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Commands.AddReviewPhoto;
using HotelSystem.Application.Features.Reviews.Commands.CreateReview;
using HotelSystem.Application.Features.Reviews.Commands.DeleteReview;
using HotelSystem.Application.Features.Reviews.Commands.RemoveReviewPhoto;
using HotelSystem.Application.Features.Reviews.Commands.UpdateReview;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Application.Features.Reviews.Queries.GetHotelReviews;
using HotelSystem.Application.Features.Reviews.Queries.GetHotelReviewSummary;
using HotelSystem.Application.Features.Reviews.Queries.GetMyReviewById;
using HotelSystem.Application.Features.Reviews.Queries.GetMyReviews;
using HotelSystem.Application.Features.Reviews.Queries.GetReviewById;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize(Roles = RoleNames.User)]
public sealed class ReviewsController(ISender sender) : ControllerBase
{
    [HttpGet("/api/hotels/{hotelId:guid}/reviews")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<ReviewDto>>> GetHotelList(Guid hotelId, [FromQuery] ReviewFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetHotelReviewsQuery(hotelId, filter), ct));

    [HttpGet("/api/hotels/{hotelId:guid}/reviews/summary")]
    [AllowAnonymous]
    public async Task<ActionResult<HotelReviewSummaryDto>> GetSummary(Guid hotelId, CancellationToken ct) =>
        Ok(await sender.Send(new GetHotelReviewSummaryQuery(hotelId), ct));

    [HttpGet("{reviewId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ReviewDto>> GetById(Guid reviewId, CancellationToken ct) =>
        Ok(await sender.Send(new GetReviewByIdQuery(reviewId), ct));

    [HttpGet("/api/account/reviews")]
    public async Task<ActionResult<PagedResult<ReviewDto>>> GetMyList([FromQuery] ReviewFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyReviewsQuery(User.RequiredUserId(), filter), ct));

    [HttpGet("/api/account/reviews/{reviewId:guid}")]
    public async Task<ActionResult<ReviewDto>> GetMineById(Guid reviewId, CancellationToken ct) =>
        Ok(await sender.Send(new GetMyReviewByIdQuery(User.RequiredUserId(), reviewId), ct));

    [HttpPost("/api/hotels/{hotelId:guid}/reviews")]
    [ProducesResponseType<CreatedResourceResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedResourceResponse>> Create(Guid hotelId, ReviewDetailsRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateReviewCommand(User.RequiredUserId(), hotelId, request.Rating, request.Comment), ct);
        return CreatedAtAction(nameof(GetMineById), new { reviewId = id }, new CreatedResourceResponse(id));
    }

    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> Update(Guid reviewId, ReviewDetailsRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateReviewCommand(User.RequiredUserId(), reviewId, request.Rating, request.Comment), ct);
        return NoContent();
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> Delete(Guid reviewId, CancellationToken ct)
    {
        await sender.Send(new DeleteReviewCommand(User.RequiredUserId(), reviewId), ct);
        return NoContent();
    }

    [HttpPost("{reviewId:guid}/photos")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
    [ProducesResponseType<PhotoDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<PhotoDto>> AddPhoto(Guid reviewId, [FromForm] PhotoUploadRequest request, CancellationToken ct)
    {
        await using var content = request.File.OpenReadStream();
        var photo = await sender.Send(new AddReviewPhotoCommand(User.RequiredUserId(), reviewId,
            new PhotoUpload(content, request.File.FileName, request.File.ContentType)), ct);
        return Created(photo.Url, photo);
    }

    [HttpDelete("{reviewId:guid}/photos/{photoId:guid}")]
    public async Task<IActionResult> RemovePhoto(Guid reviewId, Guid photoId, CancellationToken ct)
    {
        await sender.Send(new RemoveReviewPhotoCommand(User.RequiredUserId(), reviewId, photoId), ct);
        return NoContent();
    }
}
