using HotelSystem.API.Authentication;
using HotelSystem.API.Contracts.Users;
using HotelSystem.Application.Features.Users.Commands.ChangeMyPassword;
using HotelSystem.Application.Features.Users.Commands.UpdateMyAccount;
using HotelSystem.Application.Features.Users.Commands.UpdateMyAdminProfile;
using HotelSystem.Application.Features.Users.Commands.UpdateMyOwnerProfile;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Application.Features.Users.Queries.GetMyAccount;
using HotelSystem.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public sealed class AccountController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserAccountDto>> Get(CancellationToken ct) =>
        Ok(await sender.Send(new GetMyAccountQuery(User.RequiredUserId()), ct));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateAccountRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateMyAccountCommand(User.RequiredUserId(), request.Details, request.CurrentPassword), ct);
        return NoContent();
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken ct)
    {
        await sender.Send(new ChangeMyPasswordCommand(User.RequiredUserId(), request.CurrentPassword, request.NewPassword), ct);
        return NoContent();
    }

    [HttpPut("owner-profile")]
    [Authorize(Roles = RoleNames.Owner)]
    public async Task<IActionResult> UpdateOwnerProfile(UpdateOwnerProfileRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateMyOwnerProfileCommand(User.RequiredUserId(), request.CompanyName, request.BusinessPhone), ct);
        return NoContent();
    }

    [HttpPut("admin-profile")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> UpdateAdminProfile(UpdateAdminProfileRequest request, CancellationToken ct)
    {
        await sender.Send(new UpdateMyAdminProfileCommand(User.RequiredUserId(), request.EmployeeNumber, request.DepartmentName), ct);
        return NoContent();
    }
}
