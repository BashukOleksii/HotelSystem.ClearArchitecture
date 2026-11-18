using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.SearchRooms;
public sealed record SearchRoomsQuery(RoomFilter? Filter = null) : IRequest<PagedResult<RoomListItemDto>>;
