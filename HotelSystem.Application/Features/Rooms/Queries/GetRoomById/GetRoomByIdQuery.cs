using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomById;
public sealed record GetRoomByIdQuery(Guid RoomId, RoomStay? Stay = null) : IRequest<RoomDetailsDto>;
