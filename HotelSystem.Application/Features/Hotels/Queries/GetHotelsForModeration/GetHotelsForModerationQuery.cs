using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Enums;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelsForModeration;

public sealed record GetHotelsForModerationQuery(
    Guid ActorId,
    string? Search = null,
    string? City = null,
    HotelModerationStatus? Status = HotelModerationStatus.Pending,
    int Page = 1,
    int PageSize = 10)
    : IRequest<PagedResult<HotelManagementListItemDto>>;