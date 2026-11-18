using HotelSystem.Application.Features.Rooms.Models;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.UpdateRoom;
public sealed record UpdateRoomCommand(Guid ActorId, Guid RoomId, RoomDetailsInput Details) : IRequest;
