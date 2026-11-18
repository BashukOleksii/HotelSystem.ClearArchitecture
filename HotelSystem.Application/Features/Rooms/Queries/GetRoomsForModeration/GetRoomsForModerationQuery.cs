using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomsForModeration;
public sealed record GetRoomsForModerationQuery(Guid ActorId, RoomFilter? Filter = null) : IRequest<PagedResult<RoomManagementDto>>;
