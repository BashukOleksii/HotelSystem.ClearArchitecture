using HotelSystem.Application.Features.Rooms.Models;
using MediatR;

namespace HotelSystem.Application.Features.Rooms.Commands.CreateRoom;
public sealed record CreateRoomCommand(Guid ActorId, Guid HotelId, RoomDetailsInput Details) : IRequest<Guid>;
