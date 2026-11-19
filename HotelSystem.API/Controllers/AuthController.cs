using HotelSystem.API.Contracts.Users;
using HotelSystem.Application.Features.Users.Commands.Login;
using HotelSystem.Application.Features.Users.Commands.RegisterUser;
using HotelSystem.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<CreatedAccountResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreatedAccountResponse>> Register(RegisterUserRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new RegisterUserCommand(request.Details, request.Password), ct);
        return Created("/api/account", new CreatedAccountResponse(id));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenDto>> Login(LoginRequest request, CancellationToken ct) =>
        Ok(await sender.Send(new LoginCommand(request.Email, request.Password), ct));
}
