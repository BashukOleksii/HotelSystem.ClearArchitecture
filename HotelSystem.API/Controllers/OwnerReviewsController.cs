using HotelSystem.API.Authentication;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviewById;
using HotelSystem.Application.Features.Reviews.Queries.GetOwnerReviews;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/owner/reviews")]
[Authorize(Roles = RoleNames.Owner)]
public sealed class OwnerReviewsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ReviewDto>>> GetList([FromQuery] ReviewFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetOwnerReviewsQuery(User.RequiredUserId(), filter), ct));

    [HttpGet("{reviewId:guid}")]
    public async Task<ActionResult<ReviewDto>> GetById(Guid reviewId, CancellationToken ct) =>
        Ok(await sender.Send(new GetOwnerReviewByIdQuery(User.RequiredUserId(), reviewId), ct));
}
