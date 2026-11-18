using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;
using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Users.Queries.GetMyAccount;

public sealed class GetMyAccountQueryHandler(IUserRepository users, IUserProfileRepository profiles)
    : IRequestHandler<GetMyAccountQuery, UserAccountDto>
{
    public async Task<UserAccountDto> Handle(GetMyAccountQuery request, CancellationToken cancellationToken)
    {
        var user = await UserWorkflow.RequireAsync(request.ActorId, RoleNames.User, users, cancellationToken);
        return await UserWorkflow.MapAsync(user, profiles, cancellationToken);
    }
}
