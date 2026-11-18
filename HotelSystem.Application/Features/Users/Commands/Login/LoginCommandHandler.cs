using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.Login;

public sealed class LoginCommandHandler(IUserRepository users, IPasswordHasher passwords, IAccessTokenIssuer tokens)
    : IRequestHandler<LoginCommand, AccessTokenDto>
{
    public async Task<AccessTokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetByEmailAsync(request.Email.Trim(), cancellationToken);
        var validPassword = passwords.Verify(request.Password, user?.PasswordHash);
        if (!validPassword || user is null || user.IsBlocked || !user.HasRole(RoleNames.User))
            throw new AuthenticationFailedException();
        cancellationToken.ThrowIfCancellationRequested();
        return await tokens.IssueAsync(user.Id, user.Username,
            user.Roles.Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal).ToArray(), cancellationToken);
    }
}
