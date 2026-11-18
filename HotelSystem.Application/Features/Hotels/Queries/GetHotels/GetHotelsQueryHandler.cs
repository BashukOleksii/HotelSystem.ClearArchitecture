using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotels;

public sealed class GetHotelsQueryHandler(
    IHotelReadService hotelReads)
    : IRequestHandler<GetHotelsQuery, PagedResult<HotelListItemDto>>
{
    public async Task<PagedResult<HotelListItemDto>> Handle(
        GetHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var search = string.IsNullOrWhiteSpace(request.Search)
            ? null
            : request.Search.Trim();

        var city = string.IsNullOrWhiteSpace(request.City)
            ? null
            : request.City.Trim();

        return await hotelReads.GetApprovedListAsync(
            search: search,
            city: city,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);
    }
}