using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;
using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Users.Queries.GetRoles;

public sealed class GetRolesQueryHandler(IUserRepository users, IRoleRepository roles)
    : IRequestHandler<GetRolesQuery, IReadOnlyList<string>>
{
    public async Task<IReadOnlyList<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        return (await roles.GetAllAsync(cancellationToken)).Select(x => x.Name)
            .OrderBy(x => x, StringComparer.Ordinal).ToArray();
    }
}
