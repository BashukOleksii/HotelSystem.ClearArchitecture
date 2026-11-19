using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Users;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.Commands.CreateUserAsAdmin;
using HotelSystem.Application.Features.Users.Commands.GrantUserRole;
using HotelSystem.Application.Features.Users.Commands.RevokeUserRole;
using HotelSystem.Application.Features.Users.Commands.SetUserBlocked;
using HotelSystem.Application.Features.Users.Commands.UpdateUserAsAdmin;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Application.Features.Users.Queries;
using HotelSystem.Application.Features.Users.Queries.GetRoles;
using HotelSystem.Application.Features.Users.Queries.GetUserById;
using HotelSystem.Application.Features.Users.Queries.GetUsers;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = RoleNames.Admin)]
public sealed class AdminUsersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserListItemDto>>> GetUsers([FromQuery] UserFilter filter, CancellationToken ct) =>
        Ok(await sender.Send(new GetUsersQuery(User.RequiredUserId(), filter), ct));

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserAccountDto>> GetById(Guid userId, CancellationToken ct) =>
        Ok(await sender.Send(new GetUserByIdQuery(User.RequiredUserId(), userId), ct));

    [HttpPost]
    [ProducesResponseType<CreatedAccountResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedAccountResponse>> Create(RegisterUserRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateUserAsAdminCommand(User.RequiredUserId(), request.Details, request.Password), ct);
        return CreatedAtAction(nameof(GetById), new { userId = id }, new CreatedAccountResponse(id));
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, UpdateUserRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateUserAsAdminCommand(User.RequiredUserId(), userId, request.Details), ct);
        return NoContent();
    }

    [HttpPut("{userId:guid}/blocked")]
    public async Task<IActionResult> SetBlocked(Guid userId, SetUserBlockedRequest request, CancellationToken ct)
    {
        await sender.Send(new SetUserBlockedCommand(User.RequiredUserId(), userId, request.IsBlocked!.Value), ct);
        return NoContent();
    }

    [HttpPut("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> GrantRole(Guid userId, string roleName, CancellationToken ct)
    {
        await sender.Send(new GrantUserRoleCommand(User.RequiredUserId(), userId, roleName), ct);
        return NoContent();
    }

    [HttpDelete("{userId:guid}/roles/{roleName}")]
    public async Task<IActionResult> RevokeRole(Guid userId, string roleName, CancellationToken ct)
    {
        await sender.Send(new RevokeUserRoleCommand(User.RequiredUserId(), userId, roleName), ct);
        return NoContent();
    }

    [HttpGet("/api/admin/roles")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetRoles(CancellationToken ct) =>
        Ok(await sender.Send(new GetRolesQuery(User.RequiredUserId()), ct));
}
