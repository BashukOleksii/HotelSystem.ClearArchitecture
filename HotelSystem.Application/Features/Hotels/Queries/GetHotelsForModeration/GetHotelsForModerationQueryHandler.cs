using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelsForModeration;

public sealed class GetHotelsForModerationQueryHandler(
    IUserRepository users,
    IHotelReadService hotelReads)
    : IRequestHandler<GetHotelsForModerationQuery, PagedResult<HotelManagementListItemDto>>
{
    public async Task<PagedResult<HotelManagementListItemDto>> Handle(
        GetHotelsForModerationQuery request,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetByIdAsync(
            request.ActorId,
            cancellationToken);

        if (actor is null
            || actor.IsBlocked
            || !actor.HasRole(RoleNames.Admin))
        {
            throw new ForbiddenOperationException(
                "Перегляд черги модерації доступний активному користувачу з роллю Admin.");
        }

        var search = string.IsNullOrWhiteSpace(request.Search)
            ? null
            : request.Search.Trim();

        var city = string.IsNullOrWhiteSpace(request.City)
            ? null
            : request.City.Trim();

        return await hotelReads.GetModerationListAsync(
            search: search,
            city: city,
            status: request.Status,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);
    }
}