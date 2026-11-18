using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Enums;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotels;

public sealed record GetMyHotelsQuery(
    Guid ActorId,
    string? Search = null,
    string? City = null,
    HotelModerationStatus? Status = null,
    int Page = 1,
    int PageSize = 10)
    : IRequest<PagedResult<HotelManagementListItemDto>>;