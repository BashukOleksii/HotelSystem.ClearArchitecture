using HotelSystem.Application.Features.Rooms.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Queries.GetMyRoomById;
public sealed record GetMyRoomByIdQuery(Guid ActorId, Guid RoomId) : IRequest<RoomManagementDto>;
