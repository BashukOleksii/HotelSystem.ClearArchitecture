using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;
using HotelSystem.Application.Common.Models;

namespace HotelSystem.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserRepository users, IUserReadService reads)
    : IRequestHandler<GetUsersQuery, PagedResult<UserListItemDto>>
{
    public async Task<PagedResult<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        return await reads.SearchAsync((request.Filter ?? new()).Normalize(), cancellationToken);
    }
}
