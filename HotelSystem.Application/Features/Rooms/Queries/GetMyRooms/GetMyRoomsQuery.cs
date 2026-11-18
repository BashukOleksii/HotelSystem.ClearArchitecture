using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRooms;
public sealed record GetMyRoomsQuery(Guid ActorId, RoomFilter? Filter = null) : IRequest<PagedResult<RoomManagementDto>>;
