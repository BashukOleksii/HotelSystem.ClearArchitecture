using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetRoomForModerationById;
public sealed record GetRoomForModerationByIdQuery(Guid ActorId, Guid RoomId) : IRequest<RoomManagementDto>;
