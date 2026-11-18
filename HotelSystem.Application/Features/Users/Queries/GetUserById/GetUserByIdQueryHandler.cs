using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;
using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository users, IUserProfileRepository profiles)
    : IRequestHandler<GetUserByIdQuery, UserAccountDto>
{
    public async Task<UserAccountDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        var user = await UserWorkflow.TargetAsync(request.UserId, users, cancellationToken);
        return await UserWorkflow.MapAsync(user, profiles, cancellationToken);
    }
}
