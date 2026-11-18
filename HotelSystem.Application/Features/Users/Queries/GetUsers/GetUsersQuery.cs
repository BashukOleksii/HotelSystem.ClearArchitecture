using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(Guid ActorId, UserFilter? Filter = null) : IRequest<PagedResult<UserListItemDto>>;
