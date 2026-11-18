using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotels;

public sealed class GetMyHotelsQueryHandler(
    IUserRepository users,
    IHotelReadService hotelReads)
    : IRequestHandler<GetMyHotelsQuery, PagedResult<HotelManagementListItemDto>>
{
    public async Task<PagedResult<HotelManagementListItemDto>> Handle(
        GetMyHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetByIdAsync(
            request.ActorId,
            cancellationToken);

        if (actor is null
            || actor.IsBlocked
            || !actor.HasRole(RoleNames.Owner))
        {
            throw new ForbiddenOperationException(
                "Перегляд власних готелів доступний активному користувачу з роллю Owner.");
        }

        var search = string.IsNullOrWhiteSpace(request.Search)
            ? null
            : request.Search.Trim();

        var city = string.IsNullOrWhiteSpace(request.City)
            ? null
            : request.City.Trim();

        return await hotelReads.GetOwnerListAsync(
            ownerId: actor.Id,
            search: search,
            city: city,
            status: request.Status,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);
    }
}