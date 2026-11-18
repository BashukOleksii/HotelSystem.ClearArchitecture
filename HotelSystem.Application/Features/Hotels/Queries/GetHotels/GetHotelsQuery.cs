using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotels;

public sealed record GetHotelsQuery(
    string? Search = null,
    string? City = null,
    int Page = 1,
    int PageSize = 10)
    : IRequest<PagedResult<HotelListItemDto>>;