using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid ActorId, Guid UserId) : IRequest<UserAccountDto>;
