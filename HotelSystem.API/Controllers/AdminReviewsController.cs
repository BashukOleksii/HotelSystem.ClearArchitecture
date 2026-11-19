using HotelSystem.API.Authentication;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.Commands.DeleteReviewAsAdmin;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Application.Features.Reviews.Queries.GetReviewForModerationById;
using HotelSystem.Application.Features.Reviews.Queries.GetReviewsForModeration;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/admin/reviews")]
[Authorize(Roles = RoleNames.Admin)]
public sealed class AdminReviewsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ReviewModerationDto>>> GetList([FromQuery] ReviewFilter filter, CancellationToken ct,
        [FromQuery] Guid? userId = null, [FromQuery] bool includeDeleted = false) =>
        Ok(await sender.Send(new GetReviewsForModerationQuery(User.RequiredUserId(), filter, userId, includeDeleted), ct));

    [HttpGet("{reviewId:guid}")]
    public async Task<ActionResult<ReviewModerationDto>> GetById(Guid reviewId, CancellationToken ct) =>
        Ok(await sender.Send(new GetReviewForModerationByIdQuery(User.RequiredUserId(), reviewId), ct));

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> Delete(Guid reviewId, CancellationToken ct)
    {
        await sender.Send(new DeleteReviewAsAdminCommand(User.RequiredUserId(), reviewId), ct);
        return NoContent();
    }
}
